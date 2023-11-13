using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.Helper;
using FluentValidation;

namespace ComponentHub.Shared.DatabaseObjects;

public sealed class WclComponent
{
    private WclComponent(){}
    
    public WclComponent(Guid userId)
    {
        UserId = userId;
    }

    [Required] 
    [MaxLength(32)]
    public string Name { get; set; } = "New Component";
    
    [Key]
    public Guid Id { get; set; }

    [MaxLength(2500)]
    public string Source { get; set; } = string.Empty;
    
    [Required]
    [ForeignKey(nameof(User))]
    public Guid UserId { get; }
    public ApplicationUser User { get; set; } = null!;
}

public sealed class ComponentValidator : AbstractValidator<WclComponent>
{
    public ComponentValidator(ComponentHubContext context)
    {
        var maxNameLength = context.GetMaxLength<WclComponent>(nameof(WclComponent.Name), 24);

        RuleFor((component => component.Name))
            .MinimumLength(4)
            .MaximumLength(maxNameLength);

        
        var maxSourceLength = context.GetMaxLength<WclComponent>(nameof(WclComponent.Source));
        RuleFor(component => component.Source)
            .MaximumLength(maxSourceLength);

        RuleFor(component => component.UserId).NotEmpty();
    }
}