using System.ComponentModel.DataAnnotations;

namespace ComponentHub.DB.Settings;

internal sealed class DatabaseSettings
{
    public const string Section = "Database";

    [Required]
    public MySqlSettings MySql { get; set; } = default!;
}

internal sealed class MySqlSettings
{
    [Required(AllowEmptyStrings=false)]
    public string Username { get; set; } = default!;
    
    [Required(AllowEmptyStrings=false)]
    public string Password { get; set; } = default!;
    
    [Required(AllowEmptyStrings=false)]
    public string Database { get; set; } = default!;
    
    [Required(AllowEmptyStrings=false)]
    public string Server { get; set; } = default!;
}