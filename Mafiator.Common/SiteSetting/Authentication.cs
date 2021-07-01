using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{
    public class AuthenticationViewModel
    {
        [Display(Name = "احراز هویت Google")]
        public GoogleAuthentication GoogleAuthentication { get; set; }
        [Display(Name = "احراز هویت Microsoft")]
        public MicrosoftAuthentication MicrosoftAuthentication { get; set; }
    }
    public class GoogleAuthentication
    {
        [Display(Name = "ClientId")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string ClientId { get; set; }

        [Display(Name = "ClientSecret")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string ClientSecret { get; set; }

    }

    public class MicrosoftAuthentication
    {
        [Display(Name = "ClientId")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string ClientId { get; set; }

        [Display(Name = "ClientSecret")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string ClientSecret { get; set; }
    }

}