using System.ComponentModel.DataAnnotations;
using ComponentHub.Shared.Helper.Validation;
using FluentValidation;

namespace ComponentHub.Shared.Auth;

public sealed class RegisterOptions
{
    public string UserName { get; set; } = "";
}


public sealed class RegisterOptionsValidator : MudCompatibleAbstractValidator<RegisterOptions>
{
    public RegisterOptionsValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(16);
    }
    
    public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
    {
        var result = await ValidateAsync(ValidationContext<RegisterOptions>.CreateWithOptions((RegisterOptions)model, x => x.IncludeProperties(propertyName)));
        if (result.IsValid)
            return Array.Empty<string>();
        return result.Errors.Select(e => e.ErrorMessage);
    };
}