namespace ComponentHub.Server.Core.ResponseObjects;


internal abstract record BaseError(string Message);
internal sealed record Error404(string Message = "404: NotFound"): BaseError(Message);

internal sealed record Error402(string Message = "Unauthorized");