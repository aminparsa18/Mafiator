namespace Mafiator.Entities.Models;

/// <summary>
/// Reactions.
/// </summary>
public sealed class Reaction : BaseEntity
{
    /// <summary>
    /// Title.
    /// </summary>
    public string Title { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }

    /// <summary>
    /// Price.
    /// </summary>
    public decimal Price { get; set; }
}