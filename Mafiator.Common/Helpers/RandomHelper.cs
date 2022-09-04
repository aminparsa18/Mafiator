using System;
using System.Linq;

namespace Mafiator.Common.Helpers;

/// <summary>
/// Helper class for random functions.
/// </summary>
public class RandomHelper
{
    private const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";

    private static readonly Random Random = new();

    /// <summary>
    /// Generate a random string from specified characters.
    /// </summary>
    /// <param name="length">Length of generated random string.</param>
    /// <returns>Random string.</returns>
    public static string CreateRandomText(int length)
    {
        return new string(Enumerable.Repeat(chars, length).Select(s => s[Random.Next(s.Length)]).ToArray());
    }
}