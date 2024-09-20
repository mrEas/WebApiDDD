using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json; 

namespace App.Api.Middlwares
{
    public class GlobalExeptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExeptionHandler> _logger;
        public GlobalExeptionHandler(ILogger<GlobalExeptionHandler> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred: " + ex.Message);

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                // Формируем объект проблемы по RFC 7807
                ProblemDetails problemDetails = new ProblemDetails() 
                {
                    Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server internal error",
                    Detail = ex.Message,
                    Instance = context.Request.Path,
                };

                string json = JsonSerializer.Serialize(problemDetails);
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(json);
            }
        }
    }
}
