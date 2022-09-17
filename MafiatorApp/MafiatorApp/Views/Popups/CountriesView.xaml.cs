using MafiatorApp.ViewModels;
using Rg.Plugins.Popup.Pages;
using System.Threading.Tasks;
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

        //protected override async Task OnAppearingAnimationEndAsync()
        //{
        //    await base.OnAppearingAnimationEndAsync();
        //    await ((CountriesViewModel)this.BindingContext).LoadDataCommand.ExecuteAsync();
        //}

        private void InputView_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            ((CountriesViewModel) this.BindingContext).Filter(e.NewTextValue);
        }
    }
}