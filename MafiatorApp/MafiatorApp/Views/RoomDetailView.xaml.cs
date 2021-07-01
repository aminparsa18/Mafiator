using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RoomDetailView : ContentPage
    {
        public RoomDetailView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }
    }
}