
namespace Mafiator.Common.SiteSetting
{
    public class SiteSettings
    {
        public AdminUserSeed AdminUserSeed { get; set; }
        public JwtSettings JwtSettings { get; set; }
        public EmailSetting EmailSetting { get; set; }
        public SiteInfo SiteInfo { get; set; }
        public SocialLink SocialLink { get; set; }
        public GateWay GateWay { get; set; }
        public Navigation[] Navigations { get; set; }
    }

    public class AdminUserSeed
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string RoleName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
