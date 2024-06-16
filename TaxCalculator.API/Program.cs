using TaxCalculator.Application.Services;
using TaxCalculator.Domain.Interfaces;
using TaxCalculator.Infrastructure.Helpers;
using TaxCalculator.Infrastructure.Connection;
using TaxCalculator.Infrastructure.Services;
using TaxCalculator.Infrastructure.Sql.Dapper;
using TaxCalculator.Infrastructure.Sql.Models;
using SQLitePCL;
using TaxCalculator.Domain.ValueObjects;

Batteries.Init(); // Initialize SQLite

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<TaxConfig>(builder.Configuration.GetSection("TaxConfig"));

builder.Services.AddScoped<ITaxCalculationService,TaxCalculationService>();
builder.Services.AddTransient<ITaxCalculator, IncomeTaxCalculator>();
builder.Services.AddTransient<ITaxCalculator, SocialTaxCalculator>();
builder.Services.AddTransient<IHelperTaxCalculation, HelperTaxCalculation>();
builder.Services.AddTransient<ISqlQuery, SqlQuery>();
builder.Services.AddSingleton<DbConnections>();
builder.Services.AddHostedService<DBContext>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
