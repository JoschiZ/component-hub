using ComponentHub.Shared.Helper.Validation;
using ComponentHub.Shared.Results;
using FluentValidation;

namespace ComponentHub.DB.Features.Components;

public sealed record ComponentSource
{
    public Language Language { get; init; }

    public string SourceCode { get; init; }

    private ComponentSource(string sourceCode, Language language)
    {
        Language = language;
        SourceCode = sourceCode;
    }


    private static Validator _validator = new();

    public static ResultValidation<ComponentSource> TryCreate(string source, Language language)
    {
        var compSource = new ComponentSource(source, language);
        var validation = _validator.Validate(compSource);
        if (!validation.IsValid)
        {
            return validation.Errors;
        }

        return compSource;
    }


    public sealed class Validator: MudCompatibleAbstractValidator<ComponentSource>
    {
        public const int MaxSourceLength = 2000;
        public Validator()
        {
            RuleFor(source => source.SourceCode).MaximumLength(MaxSourceLength);
            RuleFor(source => source.Language).NotNull();
        }
    }
}