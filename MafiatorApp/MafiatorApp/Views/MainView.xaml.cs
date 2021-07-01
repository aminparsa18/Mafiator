using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainView : FlyoutPage
    {
        public MainView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this,false);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();
            IsPresented=false;
            MessagingCenter.Subscribe<HomeView>(this, "ShowMenu", args =>
            {
                Device.BeginInvokeOnMainThread(() => { IsPresented = true; });
            });
        }

        protected override void OnDisappearing()
        {
            MessagingCenter.Unsubscribe<HomeView>(this, "ShowMenu");
            base.OnDisappearing();
        }
    }
}