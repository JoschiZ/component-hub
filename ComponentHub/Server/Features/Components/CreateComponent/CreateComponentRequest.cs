using ComponentHub.Domain.Features.Components;

namespace ComponentHub.Server.Features.Components.CreateComponent;

public class CreateComponentRequest
{
    public string Name { get; set; } = "";
    public string SourceCode { get; set; } = "";
    public Language Language { get; set; }
    public Guid? WclComponentId { get; set; }
    public short Width { get; set; } = 2;
    public short Height { get; set; } = 2;
    public string Description { get; set; }
}