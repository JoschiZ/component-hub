using System.Text.Json.Serialization;
using ComponentHub.Domain.Core.Extensions;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Core.Validation;
using FluentValidation;


namespace ComponentHub.Domain.Features.Components;

public sealed record ComponentSource
{
    /// <summary>
    /// Clear text JavaScript Source
    /// </summary>
    [JsonPropertyName("component")]
    public SourceObject Source { get; }

    [JsonPropertyName("h")]
    public short Height { get; }

    [JsonPropertyName("w")]
    public short Width { get; }

    /// <summary>
    /// This is the id of the component object for warcraftlogs, not a database id!
    /// </summary>
    [JsonPropertyName("i")]
    public Guid WclComponentId { get; }
    
    [JsonConstructor]
    private ComponentSource(SourceObject source, short height, short width, Guid wclComponentId)
    {
        Source = source;
        Height = height;
        Width = width;
        WclComponentId = wclComponentId;
    }


    [JsonIgnore]
    private static readonly Validator ValidatorInstance = new();

    public static ResultValidation<ComponentSource> TryCreate(string source, short height = 2, short width = 1, Guid? wclComponentId = null)
    {
        wclComponentId ??= Guid.NewGuid();
        var newSourceObject = new ComponentSource(new SourceObject(source), height, width, (Guid)wclComponentId);
        var validation = ValidatorInstance.Validate(newSourceObject);
        if (!validation.IsValid)
        {
            return validation.Errors;
        }

        return newSourceObject;
    }
    
    public sealed class Validator: MudCompatibleAbstractValidator<ComponentSource>
    {
        public const int MaxSourceLength = 2000;
        public const short MaxSize = 10;
        public const short MinSize = 1;
        public Validator()
        {
            RuleFor(source => source.Source.Script).MaximumLength(MaxSourceLength);
            RuleFor(source => source.Width).InclusiveBetween(MinSize, MaxSize);
            RuleFor(source => source.Height).InclusiveBetween(MinSize, MaxSize);
            RuleFor(source => source.WclComponentId).NotEmpty();
        }
    }
}


public class SourceObject: IEquatable<SourceObject>
{
    public SourceObject(string script)
    {
        Script = script;
    }

    [JsonPropertyName("script")]
    public string Script { get; }

    public bool Equals(SourceObject? other)
    {
        if (ReferenceEquals(null, other)) return false;
        if (ReferenceEquals(this, other)) return true;
        return Script.NormalizeString() == other.Script.NormalizeString();
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((SourceObject)obj);
    }

    public override int GetHashCode()
    {
        return Script.GetHashCode();
    }
};
