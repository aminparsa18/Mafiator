using System.Threading.Tasks;
using MafiatorApp.Services;

namespace MafiatorApp.ViewModels.Base
{
    public abstract class ViewModelBase : ExtendedBindableObject
    {
        protected readonly IDialogService DialogService;
        protected readonly INavigationService NavigationService;
        protected readonly IWebApiService WebApiService;

        private bool _isBusy;
        private string _pageTitle;

        public bool IsBusy
        {
            get => _isBusy;
            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
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
            set
            {
                _pageTitle = value;
                RaisePropertyChanged(() => PageTitle);
            }
        }
    }
}
