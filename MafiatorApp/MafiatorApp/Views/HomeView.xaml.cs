using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure;
using FFImageLoading.Transformations;
using MafiatorApp.Cache;
using MafiatorApp.Models;
using MafiatorApp.Services;
using MafiatorApp.UserControls;
using Plugin.InAppBilling;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MafiatorApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomeView : ContentPage
    {
        private readonly string dayColor;
        private readonly string nightColor;

        private bool once;
        private bool menu;
        public HomeView()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            dayColor = ((Color)Application.Current.Resources["Day4"]).ToHex();
            nightColor = ((Color)Application.Current.Resources["Night4"]).ToHex();
            ProfileCircleTransformation.BorderHexColor =
                Application.Current.RequestedTheme == OSAppTheme.Dark ? nightColor : dayColor;

           
            Application.Current.RequestedThemeChanged += ThemeChanged;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (!Barrel.Current.Exists("HomeTut"))
            {
                await Task.Delay(1000);
               var targets = new Dictionary<View, string>()
                {
                    {ProfileImage,"Tap or swipe up for profile status"},
                };
                var overlay = new Overlay();
                overlay.Show(targets, new ShowCaseConfig()
                {
                    TextHorizontalPosition = HorizontalPosition.Center,
                    TextVerticalPosition = VerticalPosition.Top
                });
                Barrel.Current.Add("HomeTut",true,TimeSpan.FromDays(200));
            }
        }

        private void ThemeChanged(object sender, AppThemeChangedEventArgs e)
        {
            ProfileCircleTransformation.BorderHexColor =
                Application.Current.RequestedTheme == OSAppTheme.Dark ? nightColor : dayColor;
        }


        private void SwipeGestureRecognizer_OnSwiped(object sender, SwipedEventArgs e)
        {
            OverlayBox.IsVisible = true;
            OverlayBox.FadeTo(0.8, 600, Easing.Linear);
            ProfilePanel.TranslateTo(0, 0, 600, Easing.SpringOut);
            ProfileImage.TranslateTo(0, 200, 600, Easing.SpringIn);
            CloseBtn.FadeTo(1,500,Easing.Linear);
        }

        private void TapGestureRecognizer_OnTapped_(object sender, EventArgs e)
        {
            OverlayBox.IsVisible = true;
            OverlayBox.FadeTo(0.8, 600, Easing.Linear);
            ProfilePanel.TranslateTo(0, 0, 600, Easing.SpringOut);
            ProfileImage.TranslateTo(0, 200, 600, Easing.SpringIn);
            CloseBtn.FadeTo(1, 500, Easing.Linear);
        }

        private async void CloseBtn_OnClicked(object sender, EventArgs e)
        {
            //  Abs.RaiseChild(ProfileGrid);
            CloseBtn.FadeTo(0, 500, Easing.Linear);
            ProfilePanel.TranslateTo(0, ProfilePanel.Height - 48, 600, Easing.SpringIn);
            ProfileImage.TranslateTo(0, 0, 600, Easing.SpringOut);

            await OverlayBox.FadeTo(0, 600, Easing.Linear);
            OverlayBox.IsVisible = false;
        }

       

        protected override bool OnBackButtonPressed()
        {
            if (!once)
            {
                DependencyService.Get<IAlert>().ShortAlert("Tap back button once more to quit",MessageType.Info);
                once = true;
                Device.StartTimer(TimeSpan.FromSeconds(4), Reset);
                return true;
            }
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


        private async void GemTap_OnTapped(object sender, EventArgs e)
        {

            if (!CrossInAppBilling.IsSupported)
            {
                DependencyService.Get<IAlert>().ShortAlert("Payment not supported for your device",MessageType.Error);
                return;
            }

            try
            {
                var billing = CrossInAppBilling.Current;
                var connected = await billing.ConnectAsync();
                if (!connected)
                {
                    DependencyService.Get<IAlert>().ShortAlert("Couldnt stablish connection to google play", MessageType.Error);
                    return;
                }
                //check purchases
                var purchase = await billing.PurchaseAsync("test", ItemType.InAppPurchase);

                //possibility that a null came through.
                if (purchase == null)
                {
                    DependencyService.Get<IAlert>().ShortAlert("purchase returning empty", MessageType.Error);
                    return;
                }
                else
                {
                    //purchased!
                }

                //make additional billing calls
                await billing.DisconnectAsync();

            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                var message = string.Empty;
                switch (purchaseEx.PurchaseError)
                {
                    case PurchaseError.AppStoreUnavailable:
                        message = "Currently the app store seems to be unavailble. Try again later.";
                        break;
                    case PurchaseError.BillingUnavailable:
                        message = "Billing seems to be unavailable, please try again later.";
                        break;
                    case PurchaseError.PaymentInvalid:
                        message = "Payment seems to be invalid, please try again.";
                        break;
                    case PurchaseError.PaymentNotAllowed:
                        message = "Payment does not seem to be enabled/allowed, please try again.";
                        break;
                    default: message = purchaseEx.PurchaseError.ToString();break;
                }
                DependencyService.Get<IAlert>().ShortAlert(message, MessageType.Error);

                //Display message to user
            }
            catch (Exception ex)
            {
                DependencyService.Get<IAlert>().ShortAlert(ex.Message, MessageType.Error);
            }
           
        }
    }
}