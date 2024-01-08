using ComponentHub.Domain.Core.Validation;
using ComponentHub.Domain.Features.Users;
using FluentValidation;

namespace ComponentHub.Server.Features.Authentication;

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
        RuleFor(request => request.UserName).SetValidator(ApplicationUser.Validation.UsernameValidator);
    }
}