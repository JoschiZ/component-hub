using ComponentHub.Domain.Core.Primitives;
using Microsoft.AspNetCore.Components;

namespace ComponentHub.Client.Components.Features.RedirectHelper;

internal sealed class RedirectHelper(NavigationManager navigationManager)
{
    public void Redirect(BlazorFriendlyRedirectResult redirectResult)
    {
        navigationManager.NavigateTo(redirectResult.Route);
    }


    public void Redirect(string rout)
    {
        navigationManager.NavigateTo(rout);
    }
}