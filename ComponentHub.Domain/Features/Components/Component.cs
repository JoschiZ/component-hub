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
public sealed class Component: Entity<ComponentId>, IComparable<Component>
{
    private Component(ComponentId id): base(id) { }
    public string Name { get; private init; }
    public ComponentSource Source { get; private init; }
    public Version Version { get; private init; }

    public required ComponentEntryId ComponentEntryId { get; init; }
    public ComponentEntry? ComponentEntry { get; private set; }

    public static ResultValidation<Component> TryCreate(
        ComponentSource source,
        string name,
        ComponentEntryId componentEntryId,
        ComponentEntry? componentEntry = null,
        Version? version = null)
    {
        var newComponent = new Component(ComponentId.New())
        {
            Source = source,
            Name = name,
            Version = version ?? new Version(1, 0),
            ComponentEntryId = componentEntryId,
            ComponentEntry = componentEntry
        };

        var validator = new Validator();
        var validation = validator.Validate(newComponent);

        return validation.IsValid ? newComponent : validation.Errors;
    }

    

    public static string EncodeExportString(ComponentSource source)
    {
        var jsonObject = JsonSerializer.Serialize(source);
        return LZString.CompressToBase64(jsonObject);
    }

    public static ResultError<ComponentSource> DecodeExportString(string encodedString)
    {
        var decodedJsonObject = LZString.DecompressFromBase64(encodedString);
        if (decodedJsonObject is null)
        {
            return Error.InvalidExportString;
        }

        var sourceObject = JsonSerializer.Deserialize<ComponentSource>(decodedJsonObject);

        return sourceObject is not null ? sourceObject : Error.InvalidExportString;
    }
    
    public class Validator: AbstractValidator<Component>
    {
        public const int MaxNameLength = 40;
        public const int MinNameLength = 4;
        public Validator()
        {
            RuleFor(component => component.Name).MaximumLength(MaxNameLength).MinimumLength(MinNameLength);
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