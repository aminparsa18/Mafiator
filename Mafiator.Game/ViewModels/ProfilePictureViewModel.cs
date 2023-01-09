using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Avatars;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Avatars;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using System.Web;

namespace Mafiator.Game.ViewModels;

[QueryProperty(nameof(IsEdit), "isEdit")]
public partial class ProfilePictureViewModel : ViewModelBase
{
    private IEnumerable<AvatarResult> _avatars;
    private FileResult _photo;

    private readonly IAvatarsApiService _avatarsApiService;
    private readonly IPublisher<UpdateProfileEvent> _publisher;
    private readonly IUsersApiService _usersApiService;

    [ObservableProperty]
    private ValidatableObject<string> _displayName;

    [ObservableProperty]
    private bool _isDisplayNameValid = true;

    [ObservableProperty]
    private string _name;

    [ObservableProperty]
    private Avatar _avatar;

    [ObservableProperty]
    private ImageSource _image = Application.Current.RequestedTheme == AppTheme.Dark ? "photo_night.png" : "photo_day.png";

    [ObservableProperty]
    private bool _photoSet;

    [ObservableProperty]
    private bool _isEdit;

    public ObservableRangeCollection<AvatarResult> Avatars { get; set; }

    public ProfilePictureViewModel(INavigationService navigationService, IToastService toastService, IAvatarsApiService avatarsApiService,
        IPublisher<UpdateProfileEvent> publisher, IUsersApiService usersApiService) : base(navigationService, toastService)
    {
        _avatarsApiService = avatarsApiService;
        _publisher = publisher;
        _usersApiService = usersApiService;
        DisplayName = new ValidatableObject<string>();
        Avatars = new ObservableRangeCollection<AvatarResult>();
        LoadAvatarsCommand.ExecuteAsync(null);
        AddValidations();
    }

    private void AddValidations() => DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string>());

    [RelayCommand]
    private async Task UploadPhoto()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Uploading Image...");
        try
        {
            var blobServiceClient = new BlobServiceClient(
                "DefaultEndpointsProtocol=https;AccountName=mftor;AccountKey=pLoQjG6uKWpWe1vG+iVU+zKjYRpuM/tPKACmd/kM/AuBXHHfsvLOGKXsq96BusnCfrx/4St1INHVk4tibVLElA==;EndpointSuffix=core.windows.net");
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("avatars");
            var blobClient = blobContainerClient.GetBlobClient(HttpUtility.UrlEncode(_photo.FileName));
            await blobClient.UploadAsync(File.OpenRead(_photo.FullPath), new BlobUploadOptions());
            await SetProfilePicture(HttpUtility.UrlEncode(_photo.FileName));
        }
        catch (RequestFailedException)
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert("Upload failed,try again later", MessageType.Error);
        }
    }

    [RelayCommand]
    private async Task ChoosePhoto()
    {
        try
        {
            //   var result = await DialogService.ShowPickImageAsync();
            // if (result == LocalizationResourceManager.Current.GetValue("Camera"))
            // {
            //   photo = await MediaPicker.CapturePhotoAsync();
            //await NavigationService.NavigateToPopupAsync<CropImageViewModel>(photo.FullPath);
            //}
            // else
            //{
            _photo = await MediaPicker.PickPhotoAsync();
            //    //  await NavigationService.NavigateToPopupAsync<CropImageViewModel>(photo.FullPath);
            //}

            if (_photo == null)
                return;
            PhotoSet = true;
            Image = ImageSource.FromFile(_photo.FullPath);
        }
        catch (Exception e)
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert(e.Message, MessageType.Error);
        }
    }

    [RelayCommand]
    private async Task AvatarSelected()
    {
        if (!ValidateUpdate())
            return;
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Instance["SetPP"]);
        var response = await _usersApiService.UpdateProfile(new UpdateProfileRequest()
        {
            Image = Path.GetFileName(new Uri(Avatar.Name).LocalPath),
            Name = DisplayName.Value
        });
        await _navigationService.RemovePopupAsync();
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsMemoryPackAsync<ApiResult>();
            if (result.IsSuccess)
            {
                Barrel.Current.Add("UserImage", Avatar.Name, TimeSpan.FromDays(180));
                Barrel.Current.Empty("User");
                if (IsEdit)
                {
                    _publisher.Publish(new UpdateProfileEvent());
                    await _navigationService.RemoveLastFromBackStackAsync();
                }
                else
                    await _navigationService.NavigateToAsync(nameof(HomeViewModel));
            }
            else
                _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
        }
        else
        {
            var result = await response.Content.ReadAsMemoryPackAsync<ApiResult>();
            _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
        }
    }

    [RelayCommand]
    private async Task LoadAvatars()
    {
        var response = await _avatarsApiService.GetAllAvatars();
        if (response.IsSuccess)
        {
            _avatars = response.Data;
            Avatars.Clear();
            Avatars.AddRange(_avatars);
        }
        else
            _toastService.ShortAlert(response.Errors.ToString(), MessageType.Error);
    }

    [RelayCommand]
    private async Task Skip()
    {
        if (IsEdit)
            await _navigationService.RemoveLastFromBackStackAsync();
        else
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("...");
            await _navigationService.NavigateToAsync(nameof(HomeViewModel));
            await _navigationService.RemovePopupAsync();
        }
    }

    public void RefreshImage()
    {
        Name = Barrel.Current.Get<string>("UserImage");
        if (IsEdit)
        {
            var user = Barrel.Current.Get<UserDetailsResult>("User");
            DisplayName.Value = user.DisplayName;
            //Image = ImageSource.FromUri(new Uri(user.Image));
        }
    }

    public async Task SetProfilePicture(string image)
    {
        if (!ValidateUpdate())
            return;
        var response = await _usersApiService.UpdateProfile(new UpdateProfileRequest()
        {
            Image = image,
            Name = DisplayName.Value
        });
        var result = await response.Content.ReadAsMemoryPackAsync<ApiResult>();
        if (response.IsSuccessStatusCode)
        {
            if (result.IsSuccess)
            {
                Barrel.Current.Add("UserImage", Name, TimeSpan.FromDays(180));
                Barrel.Current.Empty("User");
                _publisher.Publish(new UpdateProfileEvent());
                await _navigationService.RemovePopupAsync();
                if (IsEdit)
                    await _navigationService.RemoveLastFromBackStackAsync();
                else
                    await _navigationService.NavigateToAsync(nameof(HomeViewModel));
            }
            else
            {
                await _navigationService.RemovePopupAsync();
                _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
            }
        }
        else
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
        }
    }

    private bool ValidateUpdate()
    {
        IsDisplayNameValid = DisplayName.Validate();
        return IsDisplayNameValid;
    }
}