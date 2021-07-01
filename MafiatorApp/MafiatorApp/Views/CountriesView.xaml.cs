using MafiatorApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CountriesView : PopupPage
    {
        public CountriesView()
        {
            InitializeComponent();
        }

        private void InputView_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((CountriesViewModel) this.BindingContext).Filter(e.NewTextValue);
        }
    }
}