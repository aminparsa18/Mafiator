namespace Mafiator.Game
{
    public class Constants
    {
        public static string VaultUrl { get; } = "https://mftor.blob.core.windows.net/mftor/";
        public static string FtpUrl { get; } = "94.130.50.78";
        public static string AndroidClientId = "562105913185-b5ihurtioohnocsncsu40nc3396uacbp.apps.googleusercontent.com";

        // These values do not need changing
        public static string Scope = "https://www.googleapis.com/auth/userinfo.email";
        public static string AuthorizeUrl = "https://accounts.google.com/o/oauth2/auth";
        public static string AccessTokenUrl = "https://oauth2.googleapis.com/token";
        public static string UserInfoUrl = "https://www.googleapis.com/oauth2/v2/userinfo";

        // Set these to reversed iOS/Android client ids, with :/oauth2redirect appended
        public static string AndroidRedirectUrl = "com.googleusercontent.apps.562105913185-b5ihurtioohnocsncsu40nc3396uacbp:/oauth2redirect";
    }
}