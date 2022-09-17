using MafiatorApp.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MafiatorApp.Services
{
    public interface INavigationService
    {
        Task NavigateToAsync<TViewModel>(bool replace = false) where TViewModel : ViewModelBase;
        Task NavigateToPageAsync(Page page, object parameter);
        Task<Page> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task<Page> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter, bool replace = false) where TViewModel : ViewModelBase;
        Task RemoveLastFromBackStackAsync();
        Task RemovePopupAsync();
    }
}