﻿using App.Api.Common.Constants;
using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace App.Api.Controllers
{
    public class ApiControllerBase : ControllerBase
    {
        protected IActionResult Problem(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                return Problem();
            }

            HttpContext.Items[HttpConstants.Errors] = errors;

            if (errors.All(e => e.Type == ErrorType.Validation))
            {
                return ValidationProblem(errors);
            }

            var firstError = errors.First();
            return Problem(firstError);
        }

        private IActionResult ValidationProblem(List<Error> errors)
        {
            var modelState = new ModelStateDictionary();

            foreach(var error in errors)
            {
                modelState.AddModelError(error.Code, error.Description);
            }
           
            return ValidationProblem(modelState);
        }

        protected IActionResult Problem(Error error)
        {
            var statusCode = error.Type switch
            {
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status500InternalServerError,
            };

            return Problem(statusCode: statusCode, title: error.Description);
        }
    }
}
