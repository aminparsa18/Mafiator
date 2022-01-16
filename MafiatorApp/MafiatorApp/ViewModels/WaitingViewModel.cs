using MafiatorApp.ViewModels.Base;
using System.Threading.Tasks;

namespace MafiatorApp.ViewModels
{
    public class WaitingViewModel:ViewModelBase
    {
        private string message;
        public string Message
        {
            get => message;
            set => SetProperty(ref message, value);
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string data)
                Message = data;
            return base.InitializeAsync(navigationData);
        }
    }
}
