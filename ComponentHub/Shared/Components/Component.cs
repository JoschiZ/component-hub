using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;
using ComponentHub.Shared.Auth;
using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Helper.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StronglyTypedIds;

namespace ComponentHub.Shared.Components;

public sealed class Component: Entity<ComponentId>
{
    private Component(){}
    private Component(ComponentId id, ApplicationUser owner): base(id){}
    
    public ComponentName Name { get; private init; } = new ComponentName("New Component");
    public ComponentSource Source { get; private init; } = ComponentSource.TryCreate("New Source", Language.JS);
    public Guid OwnerId { get; init; }
    public required ApplicationUser Owner { get; init; }
    
    public sealed class Validator: MudCompatibleAbstractValidator<Component>
    {
        public Validator(IValidator<ComponentSource> sourceValidator)
        {
            RuleFor(component => component.Source).NotNull().SetValidator(sourceValidator);
        }
    }
}

public sealed class ComponentConfiguration : IEntityTypeConfiguration<Component>
{
    public void Configure(EntityTypeBuilder<Component> builder)
    {
        builder.HasKey(component => component.Id);
        builder.Property(component => component.Id).HasConversion<ComponentId.EfCoreValueConverter>();
        builder.ComplexProperty(component => component.Source, propertyBuilder =>
        {
            propertyBuilder.Property(source => source.SourceCode).HasMaxLength(ComponentSource.MaxSourceLength);
        });
    }
}

[StronglyTypedId]
public partial struct ComponentId { }

public record ComponentName(string Value);
