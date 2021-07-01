using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Input;
using AutoMapper;
using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Models;
using MafiatorApp.Models.Api;
using MafiatorApp.Resources.Texts;
using MafiatorApp.Services;
using MafiatorApp.UserControls;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using MafiatorApp.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class ProfilePictureViewModel : ViewModelBase
    {
        private string name;

        public string Name
        {
            get => name;
            set
            {
                name = value;
                RaisePropertyChanged(() => Name);
            }
        }

        private Avatar avatar;

        public Avatar Avatar
        {
            get => avatar;
            set
            {
                avatar = value;
                RaisePropertyChanged(() => Avatar);
            }
        }

        private ImageSource image = "choose_photo.png";

        public ImageSource Image
        {
            get => image;
            set
            {
                image = value;
                RaisePropertyChanged(() => Image);
            }
        }

        private bool photoSet;

        public bool PhotoSet
        {
            get => photoSet;
            set
            {
                photoSet = value;
                RaisePropertyChanged(() => PhotoSet);
            }
        }

        public ObservableRangeCollection<Avatar> Avatars { get; set; }
        public IAsyncCommand LoadAvatarsCommand { get; set; }
        public IAsyncCommand AvatarSelectedCommand { get; set; }
        public ICommand SkipCommand { get; set; }
        public IAsyncCommand ChoosePhotoCommand { get; set; }
        public IAsyncCommand SetPhotoCommand { get; set; }
        private IEnumerable<Avatar> avatars;
        private readonly IWebApiService webApiService;
        private readonly IMapper mapper;
        private FileResult photo;

        public ProfilePictureViewModel(IWebApiService webApiService, IMapper mapper)
        {
            this.mapper = mapper;
            this.webApiService = webApiService;
            SkipCommand = new Command(Skip);
            ChoosePhotoCommand = new AsyncCommand(ChoosePhoto);
            SetPhotoCommand = new AsyncCommand(SetPhoto);
            Avatars = new ObservableRangeCollection<Avatar>();
            LoadAvatarsCommand = new AsyncCommand(LoadAvatars);
            LoadAvatarsCommand.ExecuteAsync();
            AvatarSelectedCommand = new AsyncCommand(AvatarSelected);
        }

        private async Task SetPhoto()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Uploading Image...");
            try
            {
                var blobServiceClient = new BlobServiceClient(
                    "DefaultEndpointsProtocol=https;AccountName=mftor;AccountKey=op73DprkZvRMfUzeB9TeUs+V9IviHcwlp8/4Gcj8HXtYIc5vuPP5C5bDuD+3RpTbVXkmljJDpDp1J3hP596tAA==;EndpointSuffix=core.windows.net");
                var blobContainerClient = blobServiceClient.GetBlobContainerClient("mftor");
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
                var result = await DialogService.ShowPickImageAsync();
                if (result == TextsTranslateManager.Translate("Camera"))
                {
                    photo = await MediaPicker.CapturePhotoAsync();
                    //await NavigationService.NavigateToPopupAsync<CropImageViewModel>(photo.FullPath);
                }
                else
                {
                    photo = await MediaPicker.PickPhotoAsync();
                    //  await NavigationService.NavigateToPopupAsync<CropImageViewModel>(photo.FullPath);
                }

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
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Changing profile picture");
            var response = await webApiService.UpdateProfilePicture(Avatar.Name);
            await NavigationService.RemovePopupAsync();
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
                if (result.IsSuccess)
                {
                    Barrel.Current.Add("UserImage", Avatar.Name, TimeSpan.FromDays(180));
                 //   Application.Current.MainPage = new TransitionNavigationPage(new HomeView());
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
            var response = await webApiService.GetAllAvatars();
            if (response.IsSuccess)
            {
                avatars = mapper.Map<IEnumerable<Avatar>>(response.Data);
                Avatars.Clear();
                Avatars.AddRange(avatars);
            }
            else
                DependencyService.Get<IAlert>().ShortAlert(response.Errors.ToString(), MessageType.Error);
        }

        private async void Skip()
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Starting Game...");
            await NavigationService.NavigateToAsync<HomeViewModel>(); // new TransitionNavigationPage(new HomeView());
            await NavigationService.RemovePopupAsync();
        }

        public void RefreshImage()
        {
            Name = Barrel.Current.Get<string>("UserImage");
        }

        public async Task SetProfilePicture(string name)
        {
            var response = await webApiService.UpdateProfilePicture(name);
            var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
            if (response.IsSuccessStatusCode)
            {
                if (result.IsSuccess)
                {
                    Barrel.Current.Add("UserImage", name, TimeSpan.FromDays(180));
                    await NavigationService.NavigateToAsync<HomeViewModel>();
                    await NavigationService.RemovePopupAsync();
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
    }
}