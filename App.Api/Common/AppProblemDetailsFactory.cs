using App.Api.Common.Constants;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace App.Api.Common
{
    public class AppProblemDetailsFactory : ProblemDetailsFactory
    {
        private readonly ApiBehaviorOptions _apiBehaviorOptions;//подключаемся к атрибуту [ApiController]
        public AppProblemDetailsFactory(ApiBehaviorOptions options)
        {
            _apiBehaviorOptions = options ?? throw new ArgumentNullException(nameof(options));
        }
        public override ProblemDetails CreateProblemDetails(HttpContext httpContext, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            statusCode ??= 500;

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            EnrichProblemDetails(httpContext, problemDetails, statusCode.Value);

            return problemDetails;
        }

        public override ValidationProblemDetails CreateValidationProblemDetails(HttpContext httpContext,
            ModelStateDictionary modelStateDictionary, int? statusCode = null, string? title = null, string? type = null, string? detail = null, string? instance = null)
        {
            if (modelStateDictionary == null)
            {
                throw new ArgumentNullException(nameof(modelStateDictionary));
            }

            statusCode ??= 400;

            var validationDetails = new ValidationProblemDetails
            {
                Status = statusCode,
                Title = title,
                Type = type,
                Detail = detail,
                Instance = instance
            };

            EnrichProblemDetails(httpContext, validationDetails, statusCode.Value);

            return validationDetails;
        }

        private void EnrichProblemDetails(HttpContext httpContext, ProblemDetails problemDetails, int statusCode)
        {
            problemDetails.Status ??= statusCode;
            
            if (_apiBehaviorOptions.ClientErrorMapping.TryGetValue(statusCode, out var clientErrorData)){

                problemDetails.Title ??= clientErrorData.Title;
                problemDetails.Type ??= clientErrorData.Link;
            }

            var traceId = Activity.Current?.Id ?? httpContext.TraceIdentifier;

            if (traceId is not null)
                problemDetails.Extensions.Add(HttpConstants.TraceId, traceId);

            var errors = httpContext?.Items[HttpConstants.Errors] as List<Error>;

            if(errors is not null)
            {
                problemDetails.Extensions.Add(HttpConstants.ErrorMessages, errors.Select(e=>e.Description));
            }
        }
    }
}
