using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuView : ContentPage
    {
        public MenuView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }
    }
}