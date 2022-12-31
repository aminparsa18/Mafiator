using Mafiator.Game.Resources.Texts;

namespace Mafiator.Game.Validations;

public class PhoneNoRule<T> : IValidationRule<T>
{
    public PhoneNoRule()
    {
        ValidationMessage = LocalizationResourceManager.Instance["InvalidPhoneNumber"];
    }
    public string ValidationMessage { get; set; }

    public bool Check(T value) => value == null ? false : value.ToString().StartsWith("09") && value.ToString().Length == 11;
}