using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;

namespace MafiatorApp.ViewModels
{
   public class SuggestionViewModel:ViewModelBase
    {
        public IAsyncCommand CreateCommand { get; set; }
        public IAsyncCommand SkipCommand { get; set; }

        public SuggestionViewModel()
        {
            CreateCommand=new AsyncCommand(Create);
            SkipCommand=new AsyncCommand(Skip);
        }

        private async Task Skip()
        {
            await NavigationService.NavigateToAsync<HomeViewModel>();
        }

        private async Task Create()
        {
            await NavigationService.NavigateToPopupAsync<NewRoomViewModel>();

        }
    }
}
