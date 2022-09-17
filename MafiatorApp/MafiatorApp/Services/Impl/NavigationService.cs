using MafiatorApp.ViewModels.Base;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MafiatorApp.Services.Impl
{
    public class NavigationService : INavigationService
    {
        public Task NavigateToAsync<TViewModel>(bool replace = false) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null, replace);
        }

        public Task NavigateToAsync<TViewModel>(object parameter, bool replace = false) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter, replace);
        }

        public async Task<Page> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return await InternalNavigateToPopupAsync(typeof(TViewModel), null);
        }

        public async Task<Page> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return await InternalNavigateToPopupAsync(typeof(TViewModel), parameter);
        }

        public Task NavigateToPageAsync(Page page, object parameter)
        {
            return InternalNavigateToAsync(page, parameter);
        }

        public async Task RemovePopupAsync()
        {
            await PopupNavigation.Instance.PopAsync();
        }

        public async Task RemoveLastFromBackStackAsync()
        {
            await Shell.Current.GoToAsync("..");
        }

        private static async Task InternalNavigateToAsync(Type viewModelType, object parameter, bool replace)
        {
            var page = CreatePage(viewModelType, parameter);
            var path = string.Join(string.Empty, (replace ? "//" : ""), viewModelType.Name.Replace("ViewModel", "View"));
            await Shell.Current.GoToAsync(path);
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
        }

        private static async Task<Page> InternalNavigateToPopupAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
            await PopupNavigation.Instance.PushAsync(page as PopupPage);
            return page;
        }

        private static async Task InternalNavigateToAsync(Page page, object parameter)
        {
            await Shell.Current.GoToAsync(nameof(page));
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
        }

        private static Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName?.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
            var viewType = Type.GetType(viewAssemblyName);
            return viewType;
        }

        private static Page CreatePage(Type viewModelType, object parameter)
        {
            var pageType = GetPageTypeForViewModel(viewModelType);
            if (pageType == null)
            {
                throw new Exception($"Cannot locate page type for {viewModelType}");
            }

            var page = Activator.CreateInstance(pageType) as Page;
            return page;
        }
    }
}