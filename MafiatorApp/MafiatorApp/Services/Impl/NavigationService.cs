using System;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.Views;
using Rg.Plugins.Popup.Extensions;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;
using NavigationPage = Xamarin.Forms.NavigationPage;

namespace MafiatorApp.Services.Impl
{
    public class NavigationService : INavigationService
    {
        public ViewModelBase PreviousPageViewModel
        {
            get
            {
                var mainPage = Application.Current.MainPage as HomeView;
                var viewModel = mainPage.Navigation.NavigationStack[0].BindingContext;
                return viewModel as ViewModelBase;
            }
        }

        public string PreviousPage
        {
            get
            {
                var navigation = GetNavigation();
                var previousPage = navigation.NavigationStack[2];
                return previousPage.ToString();
            }
        }


        public Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), null);
        }

        public Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            return InternalNavigateToAsync(typeof(TViewModel), parameter);
        }

        public async Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
        {
            await InternalNavigateToModalAsync(typeof(TViewModel), parameter);
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

        public async Task NavigateToRootPage()
        {
            var navigation = GetNavigation();
            await navigation.PopToRootAsync(true);
        }

        public async Task RemovePopupAsync()
        {
            var navigation = GetNavigation();
            await navigation.PopPopupAsync();
        }

        public async Task RemoveModalAsync()
        {
            var navigation = GetNavigation();
            await navigation.PopModalAsync();
        }

        public async Task RemoveLastFromBackStackAsync()
        {
            var navigation = GetNavigation();
            await navigation.PopAsync();
        }

        public Task RemoveBackStackAsync()
        {
            var navigation = GetNavigation();

            if (navigation == null) return Task.FromResult(true);
            for (var i = 0; i < navigation.NavigationStack.Count - 2; i++)
            {
                var page = navigation.NavigationStack[i];
                navigation.RemovePage(page);
            }

            return Task.FromResult(true);
        }

        private static async Task InternalNavigateToAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);
            var navigation = GetNavigation();

            await navigation.PushAsync(page);
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
        }

        private static async Task InternalNavigateToModalAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);
            var navigation = GetNavigation();
            await navigation.PushModalAsync(page);
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
        }

        private static async Task<Page> InternalNavigateToPopupAsync(Type viewModelType, object parameter)
        {
            var page = CreatePage(viewModelType, parameter);
            var navigation = GetNavigation();
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
            await navigation.PushPopupAsync(page as PopupPage);
            return page;
        }

        private static async Task InternalNavigateToAsync(Page page, object parameter)
        {
            var navigationPage = GetNavigation();
            await navigationPage.PushAsync(page);
            await ((ViewModelBase) page.BindingContext).InitializeAsync(parameter);
        }

        private static INavigation GetNavigation()
        {
            INavigation navigationPage;
            if (Application.Current.MainPage is MasterDetailPage masterDetailPage)
            {
                if (masterDetailPage.Detail is NavigationPage navPage)
                {
                    navigationPage = navPage.Navigation;
                }
                else
                {
                    var detailNavigationPage = new NavigationPage(masterDetailPage);
                    navigationPage = detailNavigationPage.Navigation;
                }
            }
            else
            {
                navigationPage = Application.Current.MainPage.Navigation;
            }

            return navigationPage;
        }

        private static Type GetPageTypeForViewModel(Type viewModelType)
        {
            var viewName = viewModelType.FullName?.Replace("Model", string.Empty);
            var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
            var viewAssemblyName =
                string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
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