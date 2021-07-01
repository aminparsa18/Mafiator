using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GameShareView : ContentPage
    {
        public GameShareView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }
    }
}