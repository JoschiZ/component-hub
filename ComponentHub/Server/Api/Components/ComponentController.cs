using ComponentHub.Shared.Auth;
using ComponentHub.Shared.DatabaseObjects;
using ComponentHub.Shared.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ComponentHub.Server.Api.Components;

[ApiController]
[Route("api/[controller]/[action]")]
internal sealed class ComponentController: ControllerBase
{
    private readonly ComponentHubContext _context;
    private readonly UserManager<ApplicationUser> _userManager;

    public ComponentController(ComponentHubContext context, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }


    [HttpPut]
    [Authorize]
    public async Task<Result<IResult>> Upload(WclComponent component)
    {
        if (component.UserId.ToString() != _userManager.GetUserId(User))
        {
            return new Error("UserIdMismatch", "UserId of component did not match the id of the logged in user");
        }
        
        await _context.Components.AddAsync(component);
         
        return Result<IResult>.CreateSuccess(Results.Ok());
    }
}