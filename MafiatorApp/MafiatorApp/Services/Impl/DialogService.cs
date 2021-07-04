using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;

namespace MafiatorApp.Services.Impl
{
    public class DialogService:IDialogService
    {
        public Task<bool> ShowConfirmedAsync()
        {
            return Application.Current.MainPage.DisplayAlert("Confirmation","Are you sure you want to continue?", "Yes","Cancel");
        }

        public Task<string> ShowPickImageAsync()
        {
           return Application.Current.MainPage.DisplayActionSheet(LocalizationResourceManager.Current.GetValue("ChoosePhoto"),
               LocalizationResourceManager.Current.GetValue("Cancel"), "", LocalizationResourceManager.Current.GetValue("Camera"),
               LocalizationResourceManager.Current.GetValue("Gallery"));

        }
    }
}
