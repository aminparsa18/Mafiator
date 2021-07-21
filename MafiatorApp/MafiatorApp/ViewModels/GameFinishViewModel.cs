using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.Views;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
   public class GameFinishViewModel:ViewModelBase
   {

        private string winner;
        public GameFinishViewModel()
        {
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is string winner)
            {
                this.winner = winner;
                await Task.Delay(8000);
                Application.Current.MainPage=new HomeView();
            }
        }
    }
}
