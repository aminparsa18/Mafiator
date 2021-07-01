using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{
    public class Navigation
    {
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Name { get; set; }
        [Display(Name = "نام کاربری")]
        [Required(ErrorMessage = "وارد نمودن {0} الزامی است.")]
        public string Link { get; set; }
    }
}