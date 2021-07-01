using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{
    public class SiteInfo
    {
        [Display(Name = "عنوان سایت")]
        public string Title { get; set; }
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Display(Name = "نام")]
        public string FirstName { get; set; }
        [Display(Name = "نام خانوادگی")]
        public string LastName { get; set; }
        [Display(Name = "لوگو")]
        public string Logo { get; set; }
        [Display(Name = "فاوآیکون")]
        public string Favicon { get; set; }
        [Display(Name = "متاتگ توضیحات")]
        public string MetaDescriptionTag { get; set; }
        [Display(Name = "پاورقی")]
        public string Footer { get; set; }
        [Display(Name = "آدرس سایت")]
        public string Url { get; set; }
        [Display(Name = "نام رنگ")]
        public string ColorName { get; set; }
    }

}