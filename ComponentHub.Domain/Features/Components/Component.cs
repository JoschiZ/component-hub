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
public sealed class Component: Entity<ComponentId>
{
        
    // TODO: Versioing
    private Component() { }
    private Component(ComponentId id): base(id) { }

    public string Name { get; private set; }
    public ComponentSource Source { get; private set; }

    public required ApplicationUser Owner { get; init; }
    
    public static ResultValidation<Component> TryCreate(string source, ApplicationUser owner, string name) => 
        ComponentSource.TryCreate(source)
            .Bind(componentSource => 
                new Component(ComponentId.New()) { Source = componentSource, Owner = owner, Name = name })
            .Validate(new Validator());
    

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
        public const int MaxNameLength = 24;
        public const int MinNameLength = 4;
        public Validator()
        {
            RuleFor(component => component.Name).MaximumLength(MaxNameLength).MinimumLength(MinNameLength);
            RuleFor(component => component.Source).SetValidator(new ComponentSource.Validator());
        }
    }

    public OneOf<Component, Error, List<ValidationFailure>> UpdateComponent(UpdateComponentRequest request)
    {
        var newSource = ComponentSource.TryCreate(request.SourceCode, request.Height, request.Width, request.WclComponentId);
        if (newSource.IsError)
        {
            return newSource.Error;
        }

        var oldSource = Source;
        if (newSource.ResultObject != Source)
        {
            Source = newSource.ResultObject;
        }

        var oldName = Name;
        if (Name != request.Name)
        {
            Name = request.Name;
        }
        var validator = new Validator();
        var validation = validator.Validate(this);
        
        if (!validation.IsValid)
        {
            Source = oldSource;
            Name = oldName;
            return validation.Errors;
        }

        return this;
    }
}

[StronglyTypedId]
public partial struct ComponentId;