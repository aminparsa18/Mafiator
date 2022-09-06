using Mafiator.Common.Helpers;
using RepoDb.Attributes;
using System;

namespace Mafiator.Data.Dtos.User;

/// <summary>
/// Validated user dto.
/// </summary>
[MessagePackObject()]
public class ValidateUserDto
{
    /// <summary>
    /// User key identifier.
    /// </summary>
    [Key(0)]
    [PropertyHandler(typeof(GuidPropertyHandler))]
    public Guid Id { get; set; }

    /// <summary>
    /// Display name.
    /// </summary>
    [Key(1)]
    public string DisplayName { get; set; }

    /// <summary>
    /// Image.
    /// </summary>
    [Key(2)]
    public string Image { get; set; }
}