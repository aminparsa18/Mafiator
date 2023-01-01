using Mafiator.Common.Client.Cache;
using Mafiator.Game.Models;
using Mafiator.Game.Services;
using Plugin.InAppBilling;

namespace Mafiator.Game.Views;

public partial class HomeView : ContentPage
{
    private readonly Color dayColor;
    private readonly Color nightColor;
    private readonly IOverlayService _overlayService;

    private bool once;

    public HomeView(IOverlayService overlayService)
    {
        InitializeComponent();
        dayColor = App.GetColorFromResource("Day4");
        nightColor = App.GetColorFromResource("Night4");
        _overlayService = overlayService;
        ProfileImage.BorderColor =
        Application.Current.RequestedTheme == AppTheme.Dark ? nightColor : dayColor;
        Application.Current.RequestedThemeChanged += ThemeChanged;
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        // if (!Barrel.Current.Exists("HomeTut"))
        //{
        // await Task.Delay(1000);
        Dispatcher.DispatchDelayed(TimeSpan.FromSeconds(2), () =>
        {
            var targets = new Dictionary<View, string>()
            {
                {ProfileImage,"Tap or swipe up for profile status"},
                {HelpBtn, "Get help" }
            };
            _overlayService.AddOverlay(targets);
        });
           
            Barrel.Current.Add("HomeTut",true,TimeSpan.FromDays(200));
       // }
    }

    private void ThemeChanged(object sender, AppThemeChangedEventArgs e)
    {
        ProfileImage.BorderColor = Application.Current.RequestedTheme == AppTheme.Dark ? nightColor : dayColor;
    }

    private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
    {
        OverlayBox.IsVisible = true;
        OverlayBox.FadeTo(0.8, 600, Easing.Linear);
        ProfilePanel.TranslateTo(0, 0, 600, Easing.SpringOut);
        ProfileImage.TranslateTo(0, 200, 600, Easing.SpringIn);
        CloseBtn.FadeTo(1, 500, Easing.Linear);
    }

    private void TapGestureRecognizer_OnTapped_(object sender, EventArgs e)
    {
        OverlayBox.IsVisible = true;
        OverlayBox.FadeTo(0.8, 600, Easing.Linear);
        ProfilePanel.TranslateTo(0, 0, 600, Easing.SpringOut);
        ProfileImage.TranslateTo(0, 200, 600, Easing.SpringIn);
        CloseBtn.FadeTo(1, 500, Easing.Linear);
    }

    private async void CloseBtn_OnClicked(object sender, TappedEventArgs e)
    {
        //  Abs.RaiseChild(ProfileGrid);
        _ = CloseBtn.FadeTo(0, 500, Easing.Linear);
        _ = ProfilePanel.TranslateTo(0, ProfilePanel.Height - 48, 600, Easing.SpringIn);
        _ = ProfileImage.TranslateTo(0, 0, 600, Easing.SpringOut);

        await OverlayBox.FadeTo(0, 600, Easing.Linear);
        OverlayBox.IsVisible = false;
    }

    protected override bool OnBackButtonPressed()
    {
        //if (!once)
        //{
        //    DependencyService.Get<IAlert>().ShortAlert("Tap back button once more to quit", MessageType.Info);
        //    once = true;
        //    Dispatcher.StartTimer(TimeSpan.FromSeconds(4), Reset);
        //    return true;
        //}
        return false;
    }

    private bool Reset()
    {
        once = false;
        return false;
    }

    private void ProfilePanel_OnSizeChanged(object sender, EventArgs e)
    {
        ProfilePanel.TranslationY = ProfilePanel.Height - 48;
    }


    private void GemTap_OnTapped(object sender, EventArgs e)
    {

        //if (!CrossInAppBilling.IsSupported)
        //{
        //    DependencyService.Get<IAlert>().ShortAlert("Payment not supported for your device",MessageType.Error);
        //    return;
        //}

        //try
        //{
        //    var billing = CrossInAppBilling.Current;
        //    var connected = await billing.ConnectAsync();
        //    if (!connected)
        //    {
        //        DependencyService.Get<IAlert>().ShortAlert("Couldnt stablish connection to google play", MessageType.Error);
        //        return;
        //    }
        //    //check purchases
        //    var purchase = await billing.PurchaseAsync("test", ItemType.InAppPurchase);

        //    //possibility that a null came through.
        //    if (purchase == null)
        //    {
        //        DependencyService.Get<IAlert>().ShortAlert("purchase returning empty", MessageType.Error);
        //        return;
        //    }
        //    else
        //    {
        //        //purchased!
        //    }

        //    //make additional billing calls
        //    await billing.DisconnectAsync();

        //}
        //catch (InAppBillingPurchaseException purchaseEx)
        //{
        //    var message = string.Empty;
        //    message = purchaseEx.PurchaseError switch
        //    {
        //        PurchaseError.AppStoreUnavailable => "Currently the app store seems to be unavailble. Try again later.",
        //        PurchaseError.BillingUnavailable => "Billing seems to be unavailable, please try again later.",
        //        PurchaseError.PaymentInvalid => "Payment seems to be invalid, please try again.",
        //        PurchaseError.PaymentNotAllowed => "Payment does not seem to be enabled/allowed, please try again.",
        //        _ => purchaseEx.PurchaseError.ToString(),
        //    };
        //    DependencyService.Get<IAlert>().ShortAlert(message, MessageType.Error);

        //    //Display message to user
        //}
        //catch (Exception ex)
        //{
        //    DependencyService.Get<IAlert>().ShortAlert(ex.Message, MessageType.Error);
        //}
    }
}