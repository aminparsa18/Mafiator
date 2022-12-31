using CommunityToolkit.Maui.Converters;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Gems;
using Mafiator.Common.Data.Dtos.Gems;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Plugin.InAppBilling;
using System.Diagnostics;

namespace Mafiator.Game.ViewModels;

public class StoreViewModel : ViewModelBase
{
    private LayoutState currentState;
    public LayoutState CurrentState
    {
        get => currentState;
        set => SetProperty(ref currentState, value);
    }

    private GemResult gem;
    public GemResult Gem
    {
        get => gem;
        set => SetProperty(ref gem, value);
    }

    public IAsyncRelayCommand LoadGemsCommand { get; set; }
    public IAsyncRelayCommand GoPremiumCommand { get; set; }
    public IAsyncRelayCommand BuyGemCommand { get; set; }
    public ObservableRangeCollection<GemResult> Gems { get; set; }

    private readonly IGemsApiService _gemsApiService;

    public StoreViewModel(INavigationService navigationService, IToastService toastService, IGemsApiService gemsApiService) 
        : base(navigationService, toastService)
    {
        _gemsApiService = gemsApiService;
        Gems = new ObservableRangeCollection<GemResult>();
        GoPremiumCommand = new AsyncRelayCommand(GoPremium);
        BuyGemCommand = new AsyncRelayCommand(BuyGem);
        LoadGemsCommand = new AsyncRelayCommand(LoadGems);
        LoadGemsCommand.ExecuteAsync(null);
    }

    private async Task LoadGems()
    {
        CurrentState = LayoutState.Loading;
        var gems = await _gemsApiService.GetAllGems();
        if (gems.IsSuccess)
        {
            Gems.AddRange(gems.Data);
            CurrentState = LayoutState.Success;
        }
        else
        {
            CurrentState = LayoutState.Error;
            _toastService.ShortAlert(gems.Errors.FirstOrDefault(), MessageType.Error);
        }
    }

    private async Task BuyGem()
    {
        var res= await Shell.Current.DisplayAlert("Confirmation", "Are you sure you want to continue?", "Yes", "Cancel");
        if (!res) 
            return;
        await PurchaseItem(Gem.Id.ToString());
    }

    private async Task GoPremium()
    {
        var res = await Shell.Current.DisplayAlert("Confirmation", "Are you sure you want to continue?", "Yes", "Cancel");
        if (!res) 
            return;
        await PurchaseItem("AdRemove");
    }

    public async Task PurchaseItem(string productId)
    {
        var billing = CrossInAppBilling.Current;
        try
        {
            var connected = await billing.ConnectAsync();
            if (!connected)
                _toastService.ShortAlert("we are offline or can't connect, don't try to purchase", MessageType.Error);

            //check purchases
            var purchase = await billing.PurchaseAsync(productId, ItemType.InAppPurchase);
            //possibility that a null came through.
            if (purchase == null)
                _toastService.ShortAlert("did not purchase", MessageType.Error);
            else if (purchase.State == PurchaseState.Purchased)
            {
                //purchased!
                if (DeviceInfo.Platform == DevicePlatform.Android)
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