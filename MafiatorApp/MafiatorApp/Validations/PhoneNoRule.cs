using MafiatorApp.Resources.Texts;

namespace MafiatorApp.Validations
{
    public class PhoneNoRule<T>: IValidationRule<T>
    {
        public PhoneNoRule()
        {
            ValidationMessage = TextsTranslateManager.Translate("InvalidPhoneNumber");
        }
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }
            return value.ToString().StartsWith("09") && value.ToString().Length==11;
        }
    }
}
