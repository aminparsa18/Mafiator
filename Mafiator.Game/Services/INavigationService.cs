using Mafiator.Game.ViewModels.Base;
using Mopups.Pages;

namespace Mafiator.Game.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>(bool replace = false) where TViewModel : ViewModelBase;
        Task NavigateToPageAsync(Page page, object parameter);
        Task<PopupPage> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task<PopupPage> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter, bool replace = false) where TViewModel : ViewModelBase;
        Task RemoveLastFromBackStackAsync();
        Task RemovePopupAsync();
    }
}