using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Contracts.Filters;

public class ValidateDtoAttribute : ActionFilterAttribute
{
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        // 1. Find the DTO in the action arguments
        var model = context.ActionArguments.Values.FirstOrDefault(arg => arg?.GetType().Name.Contains("Dto") == true);

        if (model is null)
        {
            context.Result = new BadRequestObjectResult("Missing request body.");
            return;
        }

        // 2. Resolve the validator dynamically based on the model's type
        var validatorType = typeof(IValidator<>).MakeGenericType(model.GetType());
        var validator = context.HttpContext.RequestServices.GetService(validatorType) as IValidator;

        if (validator is null)
        {
            // No validator registered, just proceed
            await next();
            return;
        }

        // 3. Validate
        var validationContext = new ValidationContext<object>(model);
        var validationResult = await validator.ValidateAsync(validationContext);

        if (!validationResult.IsValid)
        {
            // 4. Short-circuit and return 400
            var errors = validationResult.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray());

            context.Result = new BadRequestObjectResult(errors);
            return;
        }

        // 5. Proceed to controller action
        await next();
    }
}