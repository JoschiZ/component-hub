using ComponentHub.Shared.Features.Components;
using ComponentHub.Shared.Helper.Validation;
using FluentValidation;

namespace ComponentHub.Server.Features.Components;

public class CreateComponentRequestValidator : MudCompatibleAbstractValidator<CreateComponentRequest>
{
    public CreateComponentRequestValidator()
    {
        RuleFor(request => request.Name).MaximumLength(Component.Validator.MaxNameLength).MinimumLength(Component.Validator.MinNameLength).NotEmpty();
        RuleFor(request => request.SourceCode).MaximumLength(ComponentSource.Validator.MaxSourceLength).NotEmpty();
        RuleFor(request => request.Language).NotNull();
    }
}