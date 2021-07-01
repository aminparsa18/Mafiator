using System.ComponentModel.DataAnnotations;

namespace Mafiator.Entities.Enums
{
    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male = 1,
        [Display(Name = "زن")]
        Female = 2
    }
}
