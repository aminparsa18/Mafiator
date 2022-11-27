using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
{
    public class CropImageViewModel : ViewModelBase
    {
        private ImageSource _imageSource;
        public ImageSource ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource, value);
        }

        public CropImageViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService) : base(navigationService, localizer, toastService)
        {
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string path)
                ImageSource = ImageSource.FromFile(path);
            return base.InitializeAsync(navigationData);
        }
    }
}