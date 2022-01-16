using MafiatorApp.Services;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;

namespace MafiatorApp.ViewModels.Base
{
    public abstract class ViewModelBase : ObservableObject
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IWebApiService WebApiService;

        private bool _isBusy;
        private string _pageTitle;

        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        protected ViewModelBase()
        {
            DialogService = ViewModelLocator.GetService<IDialogService>();
            NavigationService = ViewModelLocator.GetService<INavigationService>();
            WebApiService = ViewModelLocator.GetService<IWebApiService>();
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
