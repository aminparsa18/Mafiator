namespace Mafiator.Entities.Models;

/// <summary>
/// Avatar picture of user.
/// </summary>
public sealed class Avatar : BaseEntity
{
    /// <summary>
    /// Avatar file name.
    /// </summary>
    public string Name { get; set; }
}