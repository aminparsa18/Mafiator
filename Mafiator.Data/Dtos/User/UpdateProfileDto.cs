namespace Mafiator.Data.Dtos.User;

/// <summary>
/// Update profile dto.
/// </summary>
[MessagePackObject()]
public class UpdateProfileDto
{
    /// <summary>
    /// Name.
    /// </summary>
    [Key(0)]
    public string Name { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    [Key(1)]
    public string Image { get; set; }
}