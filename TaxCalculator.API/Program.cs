using TaxCalculator.Application.Services;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Infrastructure.Helpers;
using TaxCalculator.Infrastructure.Connection;
using TaxCalculator.Infrastructure.Services;
using TaxCalculator.Infrastructure.Sql.Dapper;
using SQLitePCL;
using TaxCalculator.Domain.ValueObjects;
using Serilog;
using TaxCalculator.Application.Middleware;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using TaxCalculator.API.Configurations;
using TaxCalculator.Domain.Interfaces.Caching;
using TaxCalculator.Caching;
using TaxCalculator.Domain.Interfaces.Application;
using TaxCalculator.Domain.Interfaces.Infrastructure.Repositories;
using TaxCalculator.Infrastructure.Repositories;

Batteries.Init(); // Initialize SQLite

Log.Logger = new LoggerConfiguration() // initialize Serilog
    .WriteTo.Console()
    .CreateBootstrapLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<TaxConfig>(builder.Configuration.GetSection("TaxConfig"));

builder.Services.AddScoped<ITaxCalculationService, TaxCalculationService>();

builder.Services.AddTransient<ITaxCalculator, IncomeTaxCalculator>();
builder.Services.AddTransient<ITaxCalculator, SocialTaxCalculator>();
builder.Services.AddTransient<IHelperTaxCalculation, HelperTaxCalculation>();
builder.Services.AddTransient<ITaxConfigRepository, TaxConfigRepository>();
builder.Services.AddTransient<ISqlQuery, SqlQuery>();

builder.Services.AddSingleton<DbConnections>();

builder.Services.AddHostedService<HostedService>();

builder.Services.AddMemoryCache();
builder.Services.AddTransient<ICachingService, InMemoryCachingService>();

builder.Host.UseSerilog();

#region Services.Swagger.ApiVersioning
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
#endregion



var app = builder.Build();
app.UseMiddleware<ExceptionHandler>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    var apiVersionDescriptionProvider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        foreach (var description in apiVersionDescriptionProvider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", "Version: " + description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
