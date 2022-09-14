using System;

namespace Mafiator.Data.Dtos.User;

/// <summary>
/// Role dto.
/// </summary>
public sealed class RoleDetails
{
    /// <summary>
    /// Role key identifier.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Role date created.
    /// </summary>
    public DateTime? DateCreated { get; set; }

    /// <summary>
    /// Role date modified.
    /// </summary>
    public DateTime? DateModified { get; set; }

    /// <summary>
    /// Name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Users count.
    /// </summary>
    public int UsersCount { get; set; }
}