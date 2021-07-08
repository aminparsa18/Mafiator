using System;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Models.Api;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using Rg.Plugins.Popup.Services;
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
        private string phoneNo;

        public ConfirmPhoneViewModel()
        {
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
                var response = await WebApiService.ConfirmPhoneNo(new ConfirmPhoneDto()
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
                    var resres = await response.Content.ReadAsStringAsync();
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