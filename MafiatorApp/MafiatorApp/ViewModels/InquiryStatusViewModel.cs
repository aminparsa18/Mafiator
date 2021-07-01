using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;

namespace MafiatorApp.ViewModels
{
    public class InquiryStatusViewModel:ViewModelBase
    {
        private bool inquiry;

        public bool Inquiry
        {
            get => inquiry;
            set
            {
                inquiry = value;
                RaisePropertyChanged(()=>Inquiry);
            }
        }
        public IAsyncCommand PopCommand { get; set; }

        public InquiryStatusViewModel()
        {
            PopCommand=new AsyncCommand(Pop);
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is bool data)
                Inquiry = data;
            return base.InitializeAsync(navigationData);
        }
    }
}
