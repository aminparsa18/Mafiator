using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{
    public class JwtSettings
    {
        [Display(Name = "رمز امن")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string SecretKey { get; set; }

        [Display(Name = "کلید رمزنگاری")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string EncrypKey { get; set; }

        [Display(Name = "صادر کننده")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Issuer { get; set; }

        [Display(Name = "حضار")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Audience { get; set; }

        [Display(Name = "زمان پیش فرض 0")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public int NotBeforeMinutes { get; set; }

        [Display(Name = "زمان ابطال")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public int ExpirationMinutes { get; set; }

    }
}