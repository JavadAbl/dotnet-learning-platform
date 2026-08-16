using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Shared.Filters;


public class FluentValidationFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        foreach (var argument in context.ActionArguments.Values)
        {
            if (argument is null)
            {
                continue;
            }

            var validator = context.HttpContext.RequestServices
                .GetService(typeof(IValidator<>).MakeGenericType(argument.GetType()))
                as IValidator;

            if (validator is null)
            {
                continue;
            }

            var validationContext = new ValidationContext<object>(argument);

            var result = await validator.ValidateAsync(
                validationContext,
                context.HttpContext.RequestAborted
            );

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    context.ModelState.AddModelError(
                        error.PropertyName,
                        error.ErrorMessage
                    );
                }

                context.Result = new BadRequestObjectResult(
                    new ValidationProblemDetails(context.ModelState)
                );

                return;
            }
        }

        await next();
    }
}