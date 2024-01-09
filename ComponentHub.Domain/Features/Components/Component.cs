using System.Text.Json;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Extensions;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Users;
using FluentValidation;
using FluentValidation.Results;
using LZStringCSharp;
using OneOf;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Components;

/// <summary>
/// The Database Entity representing a Warcraftlogs Component.
/// </summary>
public class Component: Entity<ComponentId>, IComparable<Component>
{
    protected Component(ComponentId id): base(id) { }
    public ComponentSource Source { get; private init; }
    public Version Version { get; private init; }

    public required ComponentPageId ComponentPageId { get; init; }
    public ComponentPage? ComponentPage { get; private set; }

    public static ResultValidation<Component> TryCreate(
        ComponentSource source,
        ComponentPageId componentPageId,
        ComponentPage? componentEntry = null,
        Version? version = null)
    {
        var newComponent = new Component(ComponentId.New())
        {
            Source = source,
            Version = version ?? new Version(1, 0),
            ComponentPageId = componentPageId,
            ComponentPage = componentEntry
        };

        var validator = new Validator();
        var validation = validator.Validate(newComponent);

        return validation.IsValid ? newComponent : validation.Errors;
    }
    
    public class Validator: AbstractValidator<Component>
    {
        public const int MaxNameLength = 40;
        public const int MinNameLength = 4;
        public Validator()
        {
            RuleFor(component => component.Source).SetValidator(new ComponentSource.Validator());
        }
    }

    public int CompareTo(Component? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (ReferenceEquals(null, other)) return 1;
        return Version.CompareTo(other.Version);
    }
}

[StronglyTypedId]
public partial struct ComponentId;