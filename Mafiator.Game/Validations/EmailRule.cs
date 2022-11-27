using System.ComponentModel.DataAnnotations;

namespace Mafiator.Game.Validations
{
    public class EmailRule : IValidationRule<string>
    {
        public EmailRule()
        {
            ValidationMessage = "Phải là một địa chỉ email";
        }

        public string ValidationMessage { get; set; }

        public bool Check(string value)
        {
            return new EmailAddressAttribute().IsValid(value);
        }
    }
}
