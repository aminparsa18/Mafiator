using System;

namespace Mafiator.Data;

/// <summary>
/// Jwt options stored in app settings.
/// </summary>
public class Jwt
{
    /// <summary>
    /// Jwt secret key.
    /// </summary>
    public string Secret { get; set; }

    /// <summary>
    /// Jwt token life time.
    /// </summary>
    public TimeSpan TokenLifeTime{get; set; }
}