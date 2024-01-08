using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

namespace ComponentHub.Server.Settings;

public sealed class AuthenticationSettings
{
    public const string SectionName = "Authentication";

    [Required] 
    [ValidateObjectMembers] 
    public BattleNetSettings BattleNet { get; set; } = default!;
}


public sealed class BattleNetSettings
{
    public const string SectionName = "BattleNet";

    [Required] 
    public string ClientSecret { get; set; } = null!;

    [Required] 
    public string ClientId { get; set; } = null!;
}
