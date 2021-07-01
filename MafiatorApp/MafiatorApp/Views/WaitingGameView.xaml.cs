using MafiatorApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WaitingGameView : ContentPage
    {
        public WaitingGameView()
        {
            InitializeComponent();
        }

        protected override bool OnBackButtonPressed()
        {
            ((WaitingGameViewModel)BindingContext).StopHub();
            return base.OnBackButtonPressed();
        }
    }
}