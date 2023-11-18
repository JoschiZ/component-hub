using ComponentHub.Shared.Helper.Validation;
using FluentValidation;

namespace ComponentHub.Shared.Components;

public sealed record ComponentSource
{
    public Language Language { get; init; }

    public const int MaxSourceLength = 2000;
    public string SourceCode { get; init; }
    private ComponentSource(string sourceCode, Language language)
    {
        Language = language;
        SourceCode = sourceCode;
    }

    private static Validator _validator = new Validator();
    public static ComponentSource? TryCreate(string value, Language language)
    {
        var source = new ComponentSource(value, language);
        var validation = _validator.Validate(source);
        return validation.IsValid ? source : null;
    }

    private sealed class Validator: MudCompatibleAbstractValidator<ComponentSource>
    {
        public Validator()
        {
            RuleFor(source => source.SourceCode).MaximumLength(MaxSourceLength);
            RuleFor(source => source.Language).NotNull();
        }
    }
}