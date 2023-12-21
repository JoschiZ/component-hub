using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Users;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Components;

/// <summary>
/// All the data surrounding an component page
/// </summary>
public class ComponentEntry: AggregateRoot<ComponentEntryId>
{

    [Obsolete("EFCore Constructor")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    private ComponentEntry() : base(){}
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    
    private ComponentEntry(ComponentEntryId componentEntryId, string name, string description, Component initialComponent, ApplicationUser owner): base(componentEntryId)
    {
        Name = name;
        Description = description;
        Owner = owner;
        ComponentHistory = [initialComponent];
    }

    public string Name { get; }
    public string Description { get; }
    public DateTime CreatedAt { get; private set; } = DateTime.Now;
    public DateTime UpdatedAt { get; private set; } = DateTime.Now;

    // Relations
    /// <summary>
    /// The current (newest) component version
    /// </summary>
    public Component GetCurrentComponent()
    {
        // TODO: Yeah this is not great but whatever
        ComponentHistory.Sort();
        return ComponentHistory.Last();
    }

    public ApplicationUser Owner { get; }

    /// <summary>
    /// The Component Backlog
    /// </summary>
    public List<Component> ComponentHistory { get; }

    public IEnumerable<Comment> Comments { get; } = [];

    public static ResultValidation<ComponentEntry> TryCreate(ComponentEntryId id, string name, string description, Component component, ApplicationUser owner)
    {
        return new ComponentEntry(id, name, description, component, owner);
    }
    
    public static ResultValidation<ComponentEntry> TryCreate(
        string name, 
        string description, 
        ApplicationUser owner,
        ComponentSource source)
    {
        var newEntryId = ComponentEntryId.New();
        var component = Component.TryCreate(source, name, newEntryId);
        
        if (component.IsError)
        {
            return component.Error;
        }

        return new ComponentEntry(newEntryId, name, description, component.ResultObject, owner);
    }

    public ResultValidation<ComponentEntry> UpdateCurrentComponent(Component newComponent)
    {
        var validator = new Component.Validator();
        var validation = validator.Validate(newComponent);
        if (!validation.IsValid)
        {
            return validation.Errors;
        }
        
        Updated();
        ComponentHistory.Add(newComponent);
        return this;
    }

    private void Updated()
    {
        UpdatedAt = DateTime.Now;
    }
}


[StronglyTypedId]
public readonly partial struct ComponentEntryId { }


public class Comment : Entity<CommentId>
{
    public DateTime TimeStamp { get; }
    public string Content { get; }
    
    
    // Relations
    public ApplicationUser Owner { get; }
    public ComponentEntry ComponentEntry { get; } 
}

[StronglyTypedId]
public readonly partial struct CommentId
{
    
}