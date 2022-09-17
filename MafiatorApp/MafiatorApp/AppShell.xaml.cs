using MafiatorApp.Views;
using Xamarin.Forms;

namespace MafiatorApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();
        Routing.RegisterRoute(nameof(AdsView), typeof(AdsView));
        Routing.RegisterRoute(nameof(BarcodeScannerView), typeof(BarcodeScannerView));
        Routing.RegisterRoute(nameof(ChatView), typeof(ChatView));
        Routing.RegisterRoute(nameof(ConfirmPhoneView), typeof(ConfirmPhoneView));
        Routing.RegisterRoute(nameof(CropImageView), typeof(CropImageView));
        Routing.RegisterRoute(nameof(GameView), typeof(GameView));
        Routing.RegisterRoute(nameof(HomeView), typeof(HomeView));
        Routing.RegisterRoute(nameof(LoginView), typeof(LoginView));
        Routing.RegisterRoute(nameof(MyRoomsView), typeof(MyRoomsView));
        Routing.RegisterRoute(nameof(ProfilePictureView), typeof(ProfilePictureView));
        Routing.RegisterRoute(nameof(RoomDetailView), typeof(RoomDetailView));
        Routing.RegisterRoute(nameof(SplashScreenView), typeof(SplashScreenView));
        Routing.RegisterRoute(nameof(StoreView), typeof(StoreView));
        Routing.RegisterRoute(nameof(VideoView), typeof(VideoView));
        Routing.RegisterRoute(nameof(WaitingGameView), typeof(WaitingGameView));
    }
}