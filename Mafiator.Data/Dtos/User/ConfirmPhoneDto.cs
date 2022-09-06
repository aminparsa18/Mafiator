using System.ComponentModel.DataAnnotations;

namespace Mafiator.Data.Dtos.User;

/// <summary>
/// Confirm phone dto.
/// </summary>
[MessagePackObject]
public class ConfirmPhoneDto
{
    /// <summary>
    /// Phone number.
    /// </summary>
    [MessagePack.Key(0)]
    [Required]
    public string PhoneNo{ get; set; }

    /// <summary>
    /// Jwt token.
    /// </summary>
    [MessagePack.Key(1)]
    [Required]
    public string Token { get; set; }
}