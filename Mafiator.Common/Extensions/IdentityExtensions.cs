using Microsoft.AspNetCore.Identity;
using System.Text;

namespace Mafiator.Common.Extensions;

/// <summary>
/// Extension class for identity claims.
/// </summary>
public static class IdentityExtensions
{

    /// <summary>
    /// IdentityResult errors list to string
    /// </summary>
    public static string DumpErrors(this IdentityResult result, bool useHtmlNewLine = false)
    {
        var results = new StringBuilder();
        if (result.Succeeded) return results.ToString();
        foreach (IdentityError error in result.Errors)
        {
            string errorDescription = error.Description;
            if (string.IsNullOrWhiteSpace(errorDescription))
                continue;

            results.AppendLine(!useHtmlNewLine ? errorDescription : $"{errorDescription}<br/>");
        }
        return results.ToString();
    }
}