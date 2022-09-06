namespace Mafiator.Common.Data.Dtos.Reactions;

/// <summary>
/// Reaction dto
/// </summary>
[MessagePackObject()]
public class ReactionResult
{
    /// <summary>
    /// Reaction key Identifier.
    /// </summary>
    [Key(0)]
    public Guid Id { get; set; }

    /// <summary>
    /// Title.
    /// </summary>
    [Key(1)]
    public string Title { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    [Key(2)]
    public string Image { get; set; }
}