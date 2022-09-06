using System.ComponentModel.DataAnnotations;

namespace Mafiator.Data.Dtos.User;

/// <summary>
/// User login dto.
/// </summary>
[MessagePackObject]
public class UserLoginDto
{
    /// <summary>
    /// Username.
    /// </summary>
    [MessagePack.Key(0)]
    [Required] 
    public string Username { get; set; }
    
    /// <summary>
    /// Password.
    /// </summary>
    [MessagePack.Key(1)]
    [Required]
    public string Password { get; set; }
}