using Mafiator.Game.ViewModels.Base;
using Mopups.Pages;
using Mopups.Services;
using System.Globalization;
using System.Reflection;

namespace Mafiator.Game.Services.Impl;

public class NavigationService : INavigationService
{
    public Task NavigateToAsync(string route) => Shell.Current.GoToAsync($"///{route}", true);

    public Task NavigateToAsync(string route, IDictionary<string, object> parameters) =>
        Shell.Current.GoToAsync($"///{route}", true, parameters);

    public async Task RemoveLastFromBackStackAsync() => await Shell.Current.GoToAsync("..");

    public async Task<PopupPage> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase
    {
        return await InternalNavigateToPopupAsync(typeof(TViewModel), null);
    }

    public async Task<PopupPage> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase
    {
        return await InternalNavigateToPopupAsync(typeof(TViewModel), parameter);
    }

    public async Task RemovePopupAsync() => await MopupService.Instance.PopAsync();

    private static async Task<PopupPage> InternalNavigateToPopupAsync(Type viewModelType, object parameter)
    {
        var popup = CreatePopup(viewModelType, parameter);
        await ((ViewModelBase)popup.BindingContext).InitializeAsync(parameter);
        await MopupService.Instance.PushAsync(popup);
        return popup;
    }

    private static Type GetPageTypeForViewModel(Type viewModelType)
    {
        var viewName = viewModelType.FullName?.Replace("Model", string.Empty);
        var viewModelAssemblyName = viewModelType.GetTypeInfo().Assembly.FullName;
        var viewAssemblyName = string.Format(CultureInfo.InvariantCulture, "{0}, {1}", viewName, viewModelAssemblyName);
        var viewType = Type.GetType(viewAssemblyName);
        return viewType;
    }

    private static PopupPage CreatePopup(Type viewModelType, object parameter)
    {
        var pageType = GetPageTypeForViewModel(viewModelType);
        if (pageType == null)
            throw new Exception($"Cannot locate page type for {viewModelType}");

        var popup = Activator.CreateInstance(pageType) as PopupPage;
        return popup;
    }
}