using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;

namespace LabApi.Middlewares
{
    

    
        public class ExceptionHandlingMiddleware
        {
            private readonly RequestDelegate _next;
            private readonly ILogger<ExceptionHandlingMiddleware> _logger;

            public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
            {
                _next = next;
                _logger = logger;
            }

            public async Task InvokeAsync(HttpContext context)
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An unhandled exception has occurred.");
                    await HandleExceptionAsync(context, ex);
                }
            }

            private static Task HandleExceptionAsync(HttpContext context, Exception exception)
            {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception switch
            {
                ArgumentException => (int)HttpStatusCode.BadRequest,
                UnauthorizedAccessException => (int)HttpStatusCode.Unauthorized,
                _ => (int)HttpStatusCode.InternalServerError
            };

            var response = new
            {
                StatusCode = context.Response.StatusCode,
                Message = exception switch
                {
                    ArgumentException => "Invalid request. Please check your input and try again.",
                    UnauthorizedAccessException => "You are not authorized to perform this action.",
                    _ => "Internal Server Error. Please try again later."
                },
                Detailed = exception.Message
            };

            return context.Response.WriteAsJsonAsync(response);
        }
        }

}
