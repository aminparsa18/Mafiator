using System.ComponentModel.DataAnnotations;

namespace MafiatorApp.Validations
{
    public class ValidUrlRule : IValidationRule<string>
    {
        public ValidUrlRule()
        {
            ValidationMessage = "Phải là một URL";
        }

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            return new UrlAttribute().IsValid(value);
        }
    }
}
