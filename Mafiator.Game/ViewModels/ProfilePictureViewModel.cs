using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Avatars;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.Extensions.Localization;
using System.Web;

namespace Mafiator.Game.ViewModels;

public class ProfilePictureViewModel : ViewModelBase
{
    private ValidatableObject<string> _displayName;

    public ValidatableObject<string> DisplayName
    {
        get => _displayName;
        set => SetProperty(ref _displayName, value);
    }

    private bool _isDisplayNameValid = true;

    public bool IsDisplayNameValid
    {
        get => _isDisplayNameValid;
        set => SetProperty(ref _isDisplayNameValid, value);
    }

    private string name;

    public string Name
    {
        get => name;
        set => SetProperty(ref name, value);
    }

    private Avatar avatar;

    public Avatar Avatar
    {
        get => avatar;
        set => SetProperty(ref avatar, value);
    }

    private ImageSource image = "choose_photo.png";

    public ImageSource Image
    {
        get => image;
        set => SetProperty(ref image, value);
    }

    private bool photoSet;

    public bool PhotoSet
    {
        get => photoSet;
        set => SetProperty(ref photoSet, value);
    }

    public ObservableRangeCollection<Avatar> Avatars { get; set; }
    public IAsyncRelayCommand LoadAvatarsCommand { get; set; }
    public IAsyncRelayCommand AvatarSelectedCommand { get; set; }
    public IAsyncRelayCommand SkipCommand { get; set; }
    public IAsyncRelayCommand ChoosePhotoCommand { get; set; }
    public IAsyncRelayCommand SetPhotoCommand { get; set; }

    private IEnumerable<Avatar> avatars;
    private FileResult photo;
    private bool isEdit;

    private readonly IAvatarsApiService _avatarsApiService;
    private readonly IMapper _mapper;
    private readonly IPublisher<UpdateProfileEvent> _publisher;
    private readonly IUsersApiService _usersApiService;

    public ProfilePictureViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
        IAvatarsApiService avatarsApiService, IMapper mapper, IPublisher<UpdateProfileEvent> publisher, IUsersApiService usersApiService)
        : base (navigationService, localizer, toastService)
    {
        _avatarsApiService = avatarsApiService;
        _mapper = mapper;
        _publisher = publisher;
        _usersApiService = usersApiService;
        DisplayName = new ValidatableObject<string>();
        SkipCommand = new AsyncRelayCommand(Skip);
        ChoosePhotoCommand = new AsyncRelayCommand(ChoosePhoto);
        SetPhotoCommand = new AsyncRelayCommand(SetPhoto);
        Avatars = new ObservableRangeCollection<Avatar>();
        LoadAvatarsCommand = new AsyncRelayCommand(LoadAvatars);
        LoadAvatarsCommand.ExecuteAsync(null);
        AvatarSelectedCommand = new AsyncRelayCommand(AvatarSelected);
        AddValidations();
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is not bool)
            return base.InitializeAsync(navigationData);
        isEdit = true;
        var user = Barrel.Current.Get<UserDetailsResult>("User");
        DisplayName.Value = user.DisplayName;
        // Image = ImageSource.FromUri(new Uri(user.Image));

        return base.InitializeAsync(navigationData);
    }

    private void AddValidations()
    {
        DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string>(_localizer));
    }

    private async Task SetPhoto()
    {
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Uploading Image...");
        try
        {
            var blobServiceClient = new BlobServiceClient(
                "DefaultEndpointsProtocol=https;AccountName=mftor;AccountKey=pLoQjG6uKWpWe1vG+iVU+zKjYRpuM/tPKACmd/kM/AuBXHHfsvLOGKXsq96BusnCfrx/4St1INHVk4tibVLElA==;EndpointSuffix=core.windows.net");
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("avatars");
            var blobClient = blobContainerClient.GetBlobClient(HttpUtility.UrlEncode(photo.FileName));
            await blobClient.UploadAsync(File.OpenRead(photo.FullPath), new BlobUploadOptions());
            await SetProfilePicture(HttpUtility.UrlEncode(photo.FileName));
        }
        catch (RequestFailedException)
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert("Upload failed,try again later", MessageType.Error);
        }
    }

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
            photo = await MediaPicker.PickPhotoAsync();
            //    //  await NavigationService.NavigateToPopupAsync<CropImageViewModel>(photo.FullPath);
            //}

            if (photo == null)
                return;
            PhotoSet = true;
            Image = ImageSource.FromFile(photo.FullPath);
        }
        catch (Exception e)
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert(e.Message, MessageType.Error);
        }
    }

    private async Task AvatarSelected()
    {
        if (!ValidateUpdate())
            return;
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Changing profile picture");
        var response = await _usersApiService.UpdateProfile(new UpdateProfileRequest()
        {
            Image = Path.GetFileName(new Uri(Avatar.Name).LocalPath),
            Name = DisplayName.Value
        });
        await _navigationService.RemovePopupAsync();
        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
            if (result.IsSuccess)
            {
                Barrel.Current.Add("UserImage", Avatar.Name, TimeSpan.FromDays(180));
                Barrel.Current.Empty("User");
                if (isEdit)
                {
                    _publisher.Publish(new UpdateProfileEvent());
                    await _navigationService.RemoveLastFromBackStackAsync();
                }
                else
                    await _navigationService.NavigateToAsync<HomeViewModel>();
            }
            else
                _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
        }
        else
        {
            var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
            _toastService.ShortAlert(result.Errors.ToString(), MessageType.Error);
        }
    }

    private async Task LoadAvatars()
    {
        var response = await _avatarsApiService.GetAllAvatars();
        if (response.IsSuccess)
        {
            avatars = _mapper.Map<IEnumerable<Avatar>>(response.Data);
            Avatars.Clear();
            Avatars.AddRange(avatars);
        }
        else
            _toastService.ShortAlert(response.Errors.ToString(), MessageType.Error);
    }

    private async Task Skip()
    {
        if (isEdit)
            await _navigationService.RemoveLastFromBackStackAsync();
        else
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Starting Game...");
            await _navigationService
                .NavigateToAsync<HomeViewModel>(); // new TransitionNavigationPage(new HomeView());
            await _navigationService.RemovePopupAsync();
        }
    }

    public void RefreshImage() => Name = Barrel.Current.Get<string>("UserImage");

    public async Task SetProfilePicture(string image)
    {
        if (!ValidateUpdate())
            return;
        var response = await _usersApiService.UpdateProfile(new UpdateProfileRequest()
        {
            Image = image,
            Name = DisplayName.Value
        });
        var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
        if (response.IsSuccessStatusCode)
        {
            if (result.IsSuccess)
            {
                Barrel.Current.Add("UserImage", name, TimeSpan.FromDays(180));
                Barrel.Current.Empty("User");
                _publisher.Publish(new UpdateProfileEvent());
                await _navigationService.RemovePopupAsync();
                if (isEdit)
                    await _navigationService.RemoveLastFromBackStackAsync();
                else
                    await _navigationService.NavigateToAsync<HomeViewModel>();
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