using System.Threading.Tasks;
using MafiatorApp.ViewModels.Base;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class CropImageViewModel:ViewModelBase
    {
        private ImageSource imageSource;

        public ImageSource ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }

        public override Task InitializeAsync(object navigationData)
        {
            if(navigationData is string path)
                ImageSource=ImageSource.FromFile(path);
            return base.InitializeAsync(navigationData);
        }
    }
}
