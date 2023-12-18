namespace ComponentHub.Domain.Features.Components;

public readonly record struct GetComponentRequest(string UserName, string ComponentName);