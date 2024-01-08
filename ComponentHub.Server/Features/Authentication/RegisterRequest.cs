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
        RuleFor(request => request.UserName)
            .NotEmpty()
            .MinimumLength(ApplicationUser.ValidationConstants.MinUserNameLength)
            .MaximumLength(ApplicationUser.ValidationConstants.MaxUserNameLength)
            .Must(ApplicationUser.ValidationConstants.IsAllowedUserName)
            .WithMessage("A username may only contain the following characters: " + ApplicationUser.ValidationConstants.AllowedCharacters)
            .WithName("UsernameCharacterValidation")
            .WithErrorCode("UsernameCharactersInvalid");
    }
}