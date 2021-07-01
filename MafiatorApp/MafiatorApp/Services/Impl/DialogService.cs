using System.Threading.Tasks;
using MafiatorApp.Resources.Texts;
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
           return Application.Current.MainPage.DisplayActionSheet(TextsTranslateManager.Translate("ChoosePhoto"),
                TextsTranslateManager.Translate("Cancel"), "", TextsTranslateManager.Translate("Camera"),
                TextsTranslateManager.Translate("Gallery"));

        }
    }
}
