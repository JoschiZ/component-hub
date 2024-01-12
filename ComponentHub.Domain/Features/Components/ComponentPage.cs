using ComponentHub.Domain.Core;
using ComponentHub.Domain.Core.Interfaces;
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
    public List<ArchivedComponent> ComponentHistory { get; } = [];
    public Component Component { get; private set; } = default!;
    public List<Comment> Comments { get; } = [];

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
        var validator = new Component.Validator();
        var validation = validator.Validate(newComponent);
        if (!validation.IsValid)
        {
            return validation.Errors;
        }
        
        Updated();
        ComponentHistory.Add(ArchivedComponent.Create(Component));
        Component = newComponent;
        return this;
    }

    private void Updated()
    {
        UpdatedAt = DateTimeOffset.UtcNow;
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

