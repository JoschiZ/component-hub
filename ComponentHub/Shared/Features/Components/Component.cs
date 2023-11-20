using ComponentHub.DB.BaseClasses;
using ComponentHub.DB.Features.User;
using ComponentHub.Shared.Results;
using FluentValidation;
using StronglyTypedIds;

namespace ComponentHub.DB.Features.Components;

public sealed class Component: Entity<ComponentId>
{
    private Component() { }
    private Component(ComponentId id): base(id) { }

    public required string Name { get; init; }
    public required ComponentSource Source { get; init; }

    public required ApplicationUser Owner { get; init; }

    public static ResultValidation<Component> TryCreate(ComponentId id, string source, Language language, ApplicationUser owner, string name)
    {
        var compSource = ComponentSource.TryCreate(source, language);

        if (compSource.IsError)
        {
            return compSource.Error;    
        }
        
        return new Component(id)
        {
            Source = compSource.ResultObject,
            Owner = owner,
            Name = name
        };
    }
    
    public class Validator: AbstractValidator<Component>
    {
        public const int MaxNameLength = 24;
        public const int MinNameLength = 4;
        public Validator()
        {
            RuleFor(component => component.Name).MaximumLength(MaxNameLength).MinimumLength(MinNameLength);
            RuleFor(component => component.Source).SetValidator(new ComponentSource.Validator());
        }
    }
}

[StronglyTypedId]
public partial struct ComponentId;