using ComponentHub.Domain.Core.Validation;
using FluentValidation;

namespace ComponentHub.Domain.Features.Components;

public sealed class UpdateComponentRequestValidator: MudCompatibleAbstractValidator<UpdateComponentRequest>
{
    public UpdateComponentRequestValidator()
    {
        RuleFor(request => request.Name).MaximumLength(Component.Validator.MaxNameLength).MinimumLength(Component.Validator.MinNameLength).NotEmpty();
        RuleFor(request => request.SourceCode).MaximumLength(ComponentSource.Validator.MaxSourceLength).NotEmpty();
        RuleFor(request => request.Width).InclusiveBetween(ComponentSource.Validator.MinSize, ComponentSource.Validator.MaxSize);
        RuleFor(request => request.Height).InclusiveBetween(ComponentSource.Validator.MinSize, ComponentSource.Validator.MaxSize);
    }   
}