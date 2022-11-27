namespace Mafiator.Api.Constants;

/// <summary>
/// Contains different api url paths.
/// </summary>
public class ApiUrls
{
    public const string Prefix = "api/v{version:apiVersion}";
    public const string Avatars = $"{Prefix}/avatars";
}