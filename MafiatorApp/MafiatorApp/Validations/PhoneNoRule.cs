using Xamarin.CommunityToolkit.Helpers;

namespace MafiatorApp.Validations
{
    public class PhoneNoRule<T>: IValidationRule<T>
    {
        public PhoneNoRule()
        {
            ValidationMessage = LocalizationResourceManager.Current.GetValue("InvalidPhoneNumber");
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
