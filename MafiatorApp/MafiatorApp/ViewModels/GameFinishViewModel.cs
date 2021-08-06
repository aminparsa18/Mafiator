using System;
using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.Views;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
   public class GameFinishViewModel:ViewModelBase
   {

        private string winner;

        public string Winner
        {
            get => winner;
            set => SetProperty(ref winner, value);
        }
        private int timer;
        private double progressTimer;
        public double ProgressTimer
        {
            get => progressTimer;
            set => SetProperty(ref progressTimer, value);
        }
        public GameFinishViewModel()
        {
            Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                timer += 100;
                ProgressTimer = 100 * (double)timer / 8000;
                if (timer == 8000)
                {
                    Application.Current.MainPage = new HomeView();
                    return false;
                }
                return true;
            });
        }

        public override  Task InitializeAsync(object navigationData)
        {
            if (navigationData is string winner)
            {
                this.Winner = winner + " Wins";
            }

            return base.InitializeAsync(navigationData);
        }
    }
}
