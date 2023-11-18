using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;

namespace ComponentHub.Client.Helper.Authentication;

internal static class AuthenticationStateExtensions
{
    public static async Task<Guid> GetUserGuid(this Task<AuthenticationState> authStateTask)
    {
        var authenticationState = await authStateTask;
        var nameIdClaim = authenticationState.User.FindFirst(claim => claim.Type == ClaimTypes.NameIdentifier);
        if (nameIdClaim is null)
        {
            throw new Exception("NameIdentifierClaimNotFound");
        }

        var userGuid = new Guid(nameIdClaim.Value);
        return userGuid;
    }
}