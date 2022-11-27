using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Avatars;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using MafiatorApp.Models;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
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
        public IAsyncCommand LoadAvatarsCommand { get; set; }
        public IAsyncCommand AvatarSelectedCommand { get; set; }
        public IAsyncCommand SkipCommand { get; set; }
        public IAsyncCommand ChoosePhotoCommand { get; set; }
        public IAsyncCommand SetPhotoCommand { get; set; }

        private IEnumerable<Avatar> avatars;
        private FileResult photo;
        private bool isEdit;

        private readonly IAvatarsApiService _avatarsApiService;
        private readonly IMapper _mapper;
        private readonly IPublisher<UpdateProfileEvent> _publisher;
        private readonly IUsersApiService _usersApiService;

        public ProfilePictureViewModel(IAvatarsApiService avatarsApiService, IMapper mapper, IPublisher<UpdateProfileEvent> publisher, IUsersApiService usersApiService)
        {
            _avatarsApiService = avatarsApiService;
            _mapper = mapper;
            _publisher = publisher;
            _usersApiService = usersApiService;
            DisplayName = new ValidatableObject<string>();
            SkipCommand = new AsyncCommand(Skip);
            ChoosePhotoCommand = new AsyncCommand(ChoosePhoto);
            SetPhotoCommand = new AsyncCommand(SetPhoto);
            Avatars = new ObservableRangeCollection<Avatar>();
            LoadAvatarsCommand = new AsyncCommand(LoadAvatars);
            LoadAvatarsCommand.ExecuteAsync();
            AvatarSelectedCommand = new AsyncCommand(AvatarSelected);
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
            DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string>());
        }

        private async Task SetPhoto()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Uploading Image...");
            try
            {
                var blobServiceClient = new BlobServiceClient(
                    "DefaultEndpointsProtocol=https;AccountName=mftor;AccountKey=pLoQjG6uKWpWe1vG+iVU+zKjYRpuM/tPKACmd/kM/AuBXHHfsvLOGKXsq96BusnCfrx/4St1INHVk4tibVLElA==;EndpointSuffix=core.windows.net");
                var blobContainerClient = blobServiceClient.GetBlobContainerClient("avatars");
                var blobClient = blobContainerClient.GetBlobClient(HttpUtility.UrlEncode(photo.FileName));
                await blobClient.UploadAsync(System.IO.File.OpenRead(photo.FullPath), new BlobUploadOptions());
                await SetProfilePicture(HttpUtility.UrlEncode(photo.FileName));
            }
            catch (RequestFailedException)
            {
                await NavigationService.RemovePopupAsync();
                DependencyService.Get<IAlert>().ShortAlert("Upload failed,try again later", MessageType.Error);
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
                await NavigationService.RemovePopupAsync();
                DependencyService.Get<IAlert>().ShortAlert(e.Message, MessageType.Error);
            }
        }

        private async Task AvatarSelected()
        {
            if (!ValidateUpdate())
                return;
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Changing profile picture");
            var response = await _usersApiService.UpdateProfile(new UpdateProfileRequest()
            {
                Image = System.IO.Path.GetFileName(new Uri(Avatar.Name).LocalPath),
                Name = DisplayName.Value
            });
            await NavigationService.RemovePopupAsync();
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
                        await NavigationService.RemoveLastFromBackStackAsync();
                    }
                    else
                        await NavigationService.NavigateToAsync<HomeViewModel>();
                }
                else
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
            }
            else
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
                DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
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
                DependencyService.Get<IAlert>().ShortAlert(response.Errors.ToString(), MessageType.Error);
        }

        private async Task Skip()
        {
            if (isEdit)
                await NavigationService.RemoveLastFromBackStackAsync();
            else
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Starting Game...");
                await NavigationService
                    .NavigateToAsync<HomeViewModel>(); // new TransitionNavigationPage(new HomeView());
                await NavigationService.RemovePopupAsync();
            }
        }

        public void RefreshImage()
        {
            Name = Barrel.Current.Get<string>("UserImage");
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
            var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
            if (response.IsSuccessStatusCode)
            {
                if (result.IsSuccess)
                {
                    Barrel.Current.Add("UserImage", name, TimeSpan.FromDays(180));
                    Barrel.Current.Empty("User");
                    _publisher.Publish(new UpdateProfileEvent());
                    await NavigationService.RemovePopupAsync();
                    if (isEdit)
                    {
                        await NavigationService.RemoveLastFromBackStackAsync();
                    }
                    else
                        await NavigationService.NavigateToAsync<HomeViewModel>();
                }
                else
                {
                    await NavigationService.RemovePopupAsync();
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
                }
            }
            else
            {
                await NavigationService.RemovePopupAsync();
                DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
            }
        }

        private bool ValidateUpdate()
        {
            IsDisplayNameValid = DisplayName.Validate();
            return IsDisplayNameValid;
        }
    }
}