using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;

namespace MafiatorApp.ViewModels
{
    public class WaitingViewModel:ViewModelBase
    {
        private string message;
        public string Message
        {
            get => message;
            set
            {
                message = value;
                RaisePropertyChanged(()=>Message);
            }
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string data)
                Message = data;
            return base.InitializeAsync(navigationData);
        }
    }
}
