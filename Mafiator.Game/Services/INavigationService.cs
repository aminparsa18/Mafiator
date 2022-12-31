using Mafiator.Game.ViewModels.Base;
using Mopups.Pages;

namespace Mafiator.Game.Services;

public interface INavigationService
{
    Task NavigateToAsync(string route);
    Task NavigateToAsync(string route, IDictionary<string, object> parameter);
    Task RemoveLastFromBackStackAsync();
    Task<PopupPage> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase;
    Task<PopupPage> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
    Task RemovePopupAsync();
}