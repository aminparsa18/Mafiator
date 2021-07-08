using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using Plugin.InAppBilling;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class StoreViewModel : ViewModelBase
    {
        private LayoutState currentState;

        public LayoutState CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        private GemDto gem;

        public GemDto Gem
        {
            get => gem;
            set => SetProperty(ref gem, value);
        }

        public IAsyncCommand GoPremiumCommand { get; set; }
        public IAsyncCommand BuyGemCommand { get; set; }
        public ObservableRangeCollection<GemDto> Gems { get; set; }

        public StoreViewModel()
        {
            Gems = new ObservableRangeCollection<GemDto>();
            GoPremiumCommand = new AsyncCommand(GoPremium);
            BuyGemCommand = new AsyncCommand(BuyGem);
            LoadGems();
        }

        private async void LoadGems()
        {
            CurrentState = LayoutState.Loading;
            var gems = await WebApiService.GetAllGems();
            if (gems.IsSuccess)
            {
                Gems.AddRange(gems.Data);
                CurrentState = LayoutState.Success;
            }
            else
            {
                CurrentState = LayoutState.Error;
                DependencyService.Get<IAlert>().ShortAlert(gems.Errors.FirstOrDefault(), MessageType.Error);
            }
        }

        private async Task BuyGem()
        {
            var res=await DialogService.ShowConfirmedAsync();
            if(!res) return;
            await PurchaseItem(Gem.Id.ToString());
        }

        private async Task GoPremium()
        {
            var res = await DialogService.ShowConfirmedAsync();
            if (!res) return;
            await PurchaseItem("AdRemove");
        }

        public async Task PurchaseItem(string productId)
        {
            var billing = CrossInAppBilling.Current;
            try
            {
                var connected = await billing.ConnectAsync();
                if (!connected)
                {
                    DependencyService.Get<IAlert>().ShortAlert("we are offline or can't connect, don't try to purchase", MessageType.Error);
                }

                //check purchases
                var purchase = await billing.PurchaseAsync(productId, ItemType.InAppPurchase);
                //possibility that a null came through.
                if (purchase == null)
                {
                    DependencyService.Get<IAlert>().ShortAlert("did not purchase", MessageType.Error);
                }
                else if (purchase.State == PurchaseState.Purchased)
                {
                    //purchased!
                    if (Device.RuntimePlatform == Device.Android)
                    {
                        // Must call AcknowledgePurchaseAsync else the purchase will be refunded
                    }
                }
            }
            catch (InAppBillingPurchaseException purchaseEx)
            {
                //Billing Exception handle this based on the type
                Debug.WriteLine("Error: " + purchaseEx);
            }
            catch (Exception ex)
            {
                //Something else has gone wrong, log it
                Debug.WriteLine("Issue connecting: " + ex);
            }
            finally
            {
                await billing.DisconnectAsync();
            }
        }
    }
}