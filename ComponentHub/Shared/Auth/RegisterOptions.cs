using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace ComponentHub.Shared.Auth;

public sealed class RegisterOptions
{
    [Required]
    [StringLength(16, MinimumLength = 4, ErrorMessage = "Your ")]
    public string UserName { get; set; } = "";
}


public sealed class RegisterOptionsValidator : AbstractValidator<RegisterOptions>
{
    public RegisterOptionsValidator()
    {
        RuleFor(x => x.UserName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(16);
    }
}