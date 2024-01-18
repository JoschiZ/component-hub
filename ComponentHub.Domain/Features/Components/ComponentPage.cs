using System.Collections.Immutable;
using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Interfaces;
using ComponentHub.Domain.Core.Primitives;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Core.Validation;
using ComponentHub.Domain.Features.Users;
using FluentValidation;
using FluentValidation.Results;
using StronglyTypedIds;

namespace ComponentHub.Domain.Features.Components;

/// <summary>
/// All the data surrounding an component page
/// </summary>
public class ComponentPage: AggregateRoot<ComponentPageId>, IHasOwner
{
    private ComponentPage() : base(){}
    private ComponentPage(ComponentPageId id) : base(id){}
    public required string Name { get; init; }
    public required string Description { get; init; }
    public DateTimeOffset CreatedAt { get; } = DateTimeOffset.UtcNow;
    public DateTimeOffset UpdatedAt { get; private set; } = DateTimeOffset.UtcNow;
    public required UserId OwnerId { get; init; }
    public ApplicationUser? Owner { get; init; }
    public Component GetCurrentComponent() => Component;

    
    
    private readonly List<ArchivedComponent> _componentHistory = [];
    public IEnumerable<ArchivedComponent> ComponentHistory => _componentHistory.AsReadOnly();
    
    
    public Component Component { get; private set; } = default!;
    

    private readonly List<Comment> _comments = [];
    public IEnumerable<Comment> Comments => _comments.AsReadOnly();

    
    private readonly HashSet<ComponentTag> _tags = [];
    public IEnumerable<ComponentTag> Tags => _tags.AsEnumerable(); 

    public static ResultValidation<ComponentPage> TryCreate(ComponentPageId id, string name, string description, Component component, ApplicationUser owner)
    {
        
        var newComponent = new ComponentPage(id)
        {
            Name = name,
            Description = description,
            Component = component,
            OwnerId = owner.Id,
            Owner = owner
        };
        var validator = new Validator();
        var validationResult = validator.Validate(newComponent);
        
        return validationResult.IsValid ? newComponent : validationResult.Errors;
    }
    

    public ResultValidation<ComponentPage> UpdateCurrentComponent(Component newComponent)
    {
        if (newComponent.Version <= Component.Version)
        {
            return new ValidationFailure("Version", "New version should be bigger than the old version",
                    newComponent.Version);
        }
        
        var validator = new Component.Validator();
        var validation = validator.Validate(newComponent);
        if (!validation.IsValid)
        {
            return validation.Errors;
        }
        
        Updated();
        _componentHistory.Add(ArchivedComponent.Create(Component));
        Component = newComponent;
        return this;
    }
    

    private void Updated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
    }


    /// <summary>
    /// Adds a range of tags and returns the number of tags actually added
    /// </summary>
    /// <param name="tags"></param>
    /// <returns></returns>
    public int AddTagRange(IEnumerable<ComponentTag> tags) => tags.Count(AddTag);
    
    public bool AddTag(ComponentTag tag)
    {
        var add = _tags.Add(tag);
        if (add)
        {
            Updated();
        }

        return add;
    }

    public bool RemoveTag(ComponentTag tag)
    {
        var removal = _tags.Remove(tag);
        if (removal)
        {
            Updated();
        }
        return removal;
    }
    
    
    public class Validator : MudCompatibleAbstractValidator<ComponentPage>
    {
        public const int MaxDescriptionLength = 2000;
        
        public Validator()
        {
            RuleFor(entry => entry.Name)
                .MinimumLength(Component.Validator.MinNameLength)
                .MaximumLength(Component.Validator.MaxNameLength)
                .NotEmpty();

            RuleFor(entry => entry.Description)
                .MaximumLength(MaxDescriptionLength);

        }
    }
}

[StronglyTypedId]
public readonly partial struct ComponentPageId { }

