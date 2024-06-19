using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TaxCalculator.Application.Middleware
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;
        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            Log.Error("An unexpected error occurred.");

            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case ArgumentException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid argument provided.";
                    break;
                case InvalidOperationException _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Invalid operation.";
                    break;
                case KeyNotFoundException _:
                    statusCode = HttpStatusCode.NotFound;
                    message = "Resource not found.";
                    break;
                case Exception _:
                    statusCode = HttpStatusCode.BadRequest;
                    message = "Exception found.";
                    break;
                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "Internal Server Error.";
                    break;
            }

            context.Response.StatusCode = (int)statusCode;

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = message,
                Detailed = exception.Message
            };

            await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        }
    }
}
