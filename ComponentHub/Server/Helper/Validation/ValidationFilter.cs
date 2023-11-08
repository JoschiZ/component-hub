using FluentValidation;

namespace ComponentHub.Server.Helper.Validation;

internal sealed class ValidationFilter<T>
    (IValidator<T> validator)
    : IEndpointFilter where T: class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var validatable = context.Arguments.SingleOrDefault(x => x?.GetType() is T) as T;
        if (validatable is null)
        {
            return Results.BadRequest("Could not correctly validate");
        }

        var validation = await validator.ValidateAsync(validatable);

        if (!validation.IsValid)
        {
            return Results.BadRequest(validation.Errors.ToResponse());
        }
        
        var result = await next(context);


        return result;
    }
}