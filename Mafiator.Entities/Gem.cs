namespace Mafiator.Entities;

/// <summary>
/// Gem.
/// </summary>
public sealed class Gem : BaseEntity
{
    /// <summary>
    /// Count.
    /// </summary>
    public int Count { get; set; }

    /// <summary>
    /// Price.
    /// </summary>
    public int Price { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    public string Image { get; set; }
}