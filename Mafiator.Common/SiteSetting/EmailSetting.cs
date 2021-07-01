using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{
    public class EmailSetting
    {
        [Display(Name = "فرستنده")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Sender { get; set; }

        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Username { get; set; }

        [Display(Name = "پسورد")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Password { get; set; }

        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "ایمیل وارد شده نامعتبر است.")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Email { get; set; }

        [Display(Name = "آدرس هاست")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Host { get; set; }

        [Display(Name = "پورت")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public int Port { get; set; }
    }


}