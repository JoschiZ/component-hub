using ComponentHub.Shared.Components;
using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Results;

using StronglyTypedIds;

namespace ComponentHub.Shared.Features.Components;

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
    

}

[StronglyTypedId]
public partial struct ComponentId;