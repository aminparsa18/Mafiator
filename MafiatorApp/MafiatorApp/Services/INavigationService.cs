using MafiatorApp.ViewModels.Base;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace MafiatorApp.Services
{
    public interface INavigationService
    {
        ViewModelBase PreviousPageViewModel { get; }
        string PreviousPage{ get;}
        Task RemoveModalAsync();
        Task NavigateToAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task NavigateToPageAsync(Page page, object parameter);
        Task<Page> NavigateToPopupAsync<TViewModel>() where TViewModel : ViewModelBase;
        Task<Page> NavigateToPopupAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToModalAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToAsync<TViewModel>(object parameter) where TViewModel : ViewModelBase;
        Task NavigateToRootPage();
        Task RemoveLastFromBackStackAsync();
        Task RemovePopupAsync();
        Task RemoveBackStackAsync();
    }
}
