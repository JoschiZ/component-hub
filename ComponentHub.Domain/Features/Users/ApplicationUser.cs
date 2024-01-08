using System.Buffers;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Features.Components;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Users;

public sealed class ApplicationUser: IdentityUser<UserId>, IAggregateRoot<UserId>
{ 
    public IEnumerable<ComponentEntry> Components { get; } = new List<ComponentEntry>();
    public DateTimeOffset CreatedAt { get; init; } = DateTimeOffset.Now;
    
    public static class Validation
    {
        public const int MinUserNameLength = 4;
        public const int MaxUserNameLength = 24;
        public const string AllowedCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+#";
        private static readonly SearchValues<char> AllowedCharactersSearch = SearchValues.Create(AllowedCharacters);

        private static bool IsAllowedUserName(string name)
        {
            return !name.AsSpan().ContainsAnyExcept(AllowedCharactersSearch);
        }

        public static FluentValueValidator<string> UsernameValidator { get; } = new(
            initial =>
                initial
                    .NotEmpty()
                    .MinimumLength(MinUserNameLength)
                    .MaximumLength(MaxUserNameLength)
                    .Must(IsAllowedUserName));
    }
}

[StronglyTypedId]
public partial struct UserId
{
    
}

/// <summary>
/// A glue class to make it easy to define validation rules for single values using FluentValidation
/// You can reuse this class for all your fields, like for the credit card rules above.
/// </summary>
/// <typeparam name="T"></typeparam>
public class FluentValueValidator<T> : AbstractValidator<T>
{
    public FluentValueValidator(Action<IRuleBuilderInitial<T, T>> rule)
    {
        rule(RuleFor(x => x));
    }

    private IEnumerable<string> ValidateValue(T arg)
    {
        var result = Validate(arg);
        if (result.IsValid)
            return new string[0];
        return result.Errors.Select(e => e.ErrorMessage);
    }

    public Func<T, IEnumerable<string>> Validation => ValidateValue;
}