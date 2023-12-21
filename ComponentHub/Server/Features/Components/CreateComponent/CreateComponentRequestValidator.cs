using ComponentHub.Domain.Core.Validation;
using ComponentHub.Server.Features.Components.CreateComponent;
using FluentValidation;

namespace ComponentHub.Domain.Features.Components;

public class CreateComponentRequestValidator : MudCompatibleAbstractValidator<CreateComponentRequest>
{
    public CreateComponentRequestValidator()
    {
        RuleFor(request => request.Name).MaximumLength(Component.Validator.MaxNameLength).MinimumLength(Component.Validator.MinNameLength).NotEmpty();
        RuleFor(request => request.SourceCode).MaximumLength(ComponentSource.Validator.MaxSourceLength).NotEmpty();
        RuleFor(request => request.Language).NotNull();
    }
}