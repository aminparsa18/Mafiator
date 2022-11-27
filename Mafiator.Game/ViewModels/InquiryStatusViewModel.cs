using CommunityToolkit.Mvvm.Input;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
{
    public class InquiryStatusViewModel : ViewModelBase
    {
        private bool _inquiry;
        public bool Inquiry
        {
            get => _inquiry;
            set => SetProperty(ref _inquiry, value);
        }

        public IAsyncRelayCommand PopCommand { get; set; }

        public InquiryStatusViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService)
            : base(navigationService, localizer, toastService)
        {
            PopCommand = new AsyncRelayCommand(Pop);
        }

        private async Task Pop()
        {
            await _navigationService.RemovePopupAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is bool data)
                Inquiry = data;
            return base.InitializeAsync(navigationData);
        }
    }
}