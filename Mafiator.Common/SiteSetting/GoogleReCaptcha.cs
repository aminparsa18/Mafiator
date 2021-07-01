using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{

    public class GoogleReCaptcha
    {
        [Display(Name = "ClientKey")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string ClientKey { get; set; }
        [Display(Name = "SecretKey")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string SecretKey { get; set; }
    }

}