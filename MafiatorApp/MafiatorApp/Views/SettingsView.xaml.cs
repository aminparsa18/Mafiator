using MafiatorApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SettingsView : PopupPage
    {
        public SettingsView()
        {
            InitializeComponent();
            ((SettingsViewModel) this.BindingContext).Initial = false;
        }

    }
}