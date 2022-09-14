using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using Rg.Plugins.Popup.Services;
using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class ConfirmPhoneViewModel : ViewModelBase
    {
        private ValidatableObject<string> code;
        public ValidatableObject<string> Code
        {
            get => code;
            set => SetProperty(ref code, value);
        }

        private bool isCodeValid;
        public bool IsCodeValid
        {
            get => isCodeValid;
            set => SetProperty(ref isCodeValid, value);
        }

        public IAsyncCommand ConfirmCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }

        private readonly IUsersApiService _usersApiService;
        private string phoneNo;

        public ConfirmPhoneViewModel(IUsersApiService usersApiService)
        {
            _usersApiService = usersApiService;
            ConfirmCommand = new AsyncCommand(Confirm);
            PopCommand = new AsyncCommand(Pop);
            Code = new ValidatableObject<string>();
            AddValidations();
            IsCodeValid = true;
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is string phoneNo)
                this.phoneNo = phoneNo;
            return base.InitializeAsync(navigationData);
        }

        private async Task Confirm()
        {
            var isValid = CodeValidate();
            if (isValid)
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Checking Code...");
                var response = await _usersApiService.ConfirmPhoneNo(new ConfirmPhoneRequest()
                    {PhoneNo = phoneNo, Token = Code.Value});
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsMessagePackAsync<AuthResult>();
                    if (result.IsSuccess)
                    {
                        Barrel.Current.Add("Token", result.Token, TimeSpan.FromMinutes(6));
                        Barrel.Current.Add("RefreshToken", result.RefreshToken, TimeSpan.FromDays(150));
                        BaseHttpClient.Instance.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", result.Token);
                        await NavigationService.NavigateToAsync<ProfilePictureViewModel>();
                    }
                    else
                    {
                        DependencyService.Get<IAlert>()
                            .ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                    }
                }
                else
                {
                    var result = await response.Content.ReadAsMessagePackAsync<AuthResult>();
                    DependencyService.Get<IAlert>()
                        .ShortAlert(string.Join(',',result.Errors), MessageType.Error);
                }
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                DependencyService.Get<IAlert>()
                    .ShortAlert("کد فعالسازی را وارد کنید", MessageType.Error);
            }
        }

        private async Task Pop()
        {
            await NavigationService.RemoveLastFromBackStackAsync();
        }

        private void AddValidations()
        {
            Code.Validations.Add(new IsNotNullOrEmptyRule<string>
            {
                ValidationMessage = "کد فعالسازی را وارد کنید"
            });
        }

        private bool CodeValidate()
        {
            IsCodeValid = Code.Validate();
            return IsCodeValid;
        }
    }
}