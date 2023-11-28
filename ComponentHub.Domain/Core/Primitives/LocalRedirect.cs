namespace ComponentHub.Domain.Core.Primitives;

public sealed class LocalRedirect
{
    public RedirectType RedirectType { get; set; } = RedirectType.Simple;
    public string Route { get; set; } = "/";
}

public enum RedirectType
{
    Simple,
    WithMessage
}