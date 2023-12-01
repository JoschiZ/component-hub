using ComponentHub.Domain.Core.Primitives;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace ComponentHub.Client.Core;

internal sealed class RedirectHelper(NavigationManager navigationManager)
{
    public void Redirect(BlazorFriendlyRedirectResult redirectResult)
    {
        switch (redirectResult.RedirectType)
        {
            case RedirectType.Simple:
                Redirect(redirectResult.Route);
                return;
            case RedirectType.WithQuery:
                Redirect(redirectResult.Route, redirectResult.Query);
                return;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    public void Redirect(string route)
    {
        navigationManager.NavigateTo(route);
    }

    public void Redirect(string route, Dictionary<string, string?> queryParams)
    {
        navigationManager.NavigateTo(QueryHelpers.AddQueryString(route, queryParams));
    }
}