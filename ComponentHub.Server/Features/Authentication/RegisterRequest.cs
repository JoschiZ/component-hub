using ComponentHub.Domain.Core.Validation;
using FluentValidation;

namespace ComponentHub.Domain.Features.Authentication;

/// <summary>
/// The request DTO to register a user.
/// </summary>
/// <remarks>
/// Should be a record for immutability, but can't because it's recycled as a Blazor WASM Form Model
/// </remarks>
/// <param name="UserName"></param>
public class RegisterRequest()
{
    public string UserName { get; set; } = "";
};

public class RegisterRequestValidator : MudCompatibleAbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(request => request.UserName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(16);
    }
}