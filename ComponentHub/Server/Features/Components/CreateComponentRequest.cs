using ComponentHub.Shared.Components;

namespace ComponentHub.Server.Features.Components;

public class CreateComponentRequest
{
    public string Name { get; set; } = "";
    public string SourceCode { get; set; } = "";
    public Language Language { get; set; }
}