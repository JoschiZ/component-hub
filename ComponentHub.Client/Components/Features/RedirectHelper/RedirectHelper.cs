using ComponentHub.Domain.Core.Primitives;
using Microsoft.AspNetCore.Components;

namespace ComponentHub.Client.Components.Features.RedirectHelper;

internal sealed class RedirectHelper(NavigationManager navigationManager)
{
    public void Redirect(LocalRedirect redirect)
    {
        navigationManager.NavigateTo(redirect.Route);
    }


    public void Redirect(string rout)
    {
        navigationManager.NavigateTo(rout);
    }
}