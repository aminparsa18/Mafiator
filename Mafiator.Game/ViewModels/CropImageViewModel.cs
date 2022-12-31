using CommunityToolkit.Mvvm.ComponentModel;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class CropImageViewModel : ViewModelBase
{
    [ObservableProperty]
    private ImageSource _imageSource;

    public CropImageViewModel(INavigationService navigationService, IToastService toastService) : base(navigationService, toastService)
    {
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is string path)
            ImageSource = ImageSource.FromFile(path);
        return base.InitializeAsync(navigationData);
    }
}