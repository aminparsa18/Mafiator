using System.Linq;
using MafiatorApp.Resources.Texts;

namespace MafiatorApp.Validations
{
    public class PasswordRule<T>:IValidationRule<T>
    {
        public PasswordRule()
        {
            ValidationMessage = TextsTranslateManager.Translate("InvalidPassword");
        }
        public string ValidationMessage { get; set; }
        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            return value.ToString().Any(char.IsDigit) &&
                   value.ToString().Any(char.IsLetter) &&
                   value.ToString().Length > 5;
        }
    }
}
