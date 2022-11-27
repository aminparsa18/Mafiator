using Mafiator.Game.Resources.Texts;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.Validations
{
    public class PhoneNoRule<T> : IValidationRule<T>
    {
        public PhoneNoRule(IStringLocalizer<AppResources> localizer)
        {
            ValidationMessage = localizer["InvalidPhoneNumber"];
        }
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
                return false;
            return value.ToString().StartsWith("09") && value.ToString().Length == 11;
        }
    }
}