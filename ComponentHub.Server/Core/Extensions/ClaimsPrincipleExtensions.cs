using System.Security.Claims;
using ComponentHub.Domain.Core.Primitives.Results;
using ComponentHub.Domain.Features.Users;

namespace ComponentHub.Server.Core.Extensions;

internal static class ClaimsPrincipleExtensions
{
    public static ResultError<UserId> GetUserId(this ClaimsPrincipal principal)
    {
        var id = principal.FindFirstValue(ClaimTypes.NameIdentifier);
        if (id is null)
        {
            return new Error("IdNotFound", "Could not find the NameIdentifier on the principle");
        }

        return new UserId(new Guid(id));
    }
}