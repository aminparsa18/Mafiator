using CommunityToolkit.Mvvm.ComponentModel;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels.Base
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly INavigationService _navigationService;
        protected readonly IStringLocalizer<AppResources> _localizer;
        protected readonly IToastService _toastService;

        private bool _isBusy;
        private string _pageTitle;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        protected ViewModelBase(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService)
        {
            _navigationService = navigationService;
            _localizer = localizer;
            _toastService = toastService;
        }

        public virtual Task InitializeAsync(object navigationData)
        {
            return Task.FromResult(false);
        }

        public string PageTitle
        {
            get => _pageTitle;
            set => SetProperty(ref _pageTitle, value);
        }
    }
}