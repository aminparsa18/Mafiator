using System.ComponentModel.DataAnnotations;

namespace Mafiator.Common.SiteSetting
{

    public class SocialLink
    {
        [Display(Name = "یوتیوب")]
        public string Youtube { get; set; }
        [Display(Name = "فیسبوک")]
        public string Facebook { get; set; }
        [Display(Name = "اینستاگرام")]
        public string Instagram { get; set; }
        [Display(Name = "گوگل پلاس")]
        public string GooglePlus { get; set; }
        [Display(Name = "توئیتر")]
        public string Twitter { get; set; }
        [Display(Name = "تلگرام")]
        public string Telegram { get; set; }
        [Display(Name = "لینکدین")]
        public string Linkedin { get; set; }
        [Display(Name = "واتساپ")]
        public string WhatsAppChat { get; set; }
        [Display(Name = "ایمیل")]
        public string Mail { get; set; }
        [Display(Name = "شماره تماس")]
        public string Tel { get; set; }
    }

}