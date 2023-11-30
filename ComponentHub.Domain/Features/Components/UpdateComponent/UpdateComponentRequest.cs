namespace ComponentHub.Domain.Features.Components;

public sealed class UpdateComponentRequest
{
    public UpdateComponentRequest(ComponentId componentId)
    {
        ComponentId = componentId;
    }

    public ComponentId ComponentId { get; }
    public string Name { get; set; } = "";
    public string SourceCode { get; set; } = "";
    
    public short Width { get; set; }
    
    public short Height { get; set; }
    
    /// <summary>
    /// On Wcl each component has an arbitrary id, which seems to be irrelevant
    /// </summary>
    public Guid WclComponentId { get; set; } = Guid.NewGuid();
}