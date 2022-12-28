using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;
using System.Net.Http.Headers;

namespace Mafiator.Game.ViewModels;

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

    public IAsyncRelayCommand ConfirmCommand { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }

    private readonly IUsersApiService _usersApiService;
    private string _phoneNo;

    public ConfirmPhoneViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer,
        IToastService toastService, IUsersApiService usersApiService) : base(navigationService, localizer, toastService)
    {
        _usersApiService = usersApiService;
        ConfirmCommand = new AsyncRelayCommand(Confirm);
        PopCommand = new AsyncRelayCommand(Pop);
        Code = new ValidatableObject<string>();
        AddValidations();
        IsCodeValid = true;
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is string phoneNo)
            _phoneNo = phoneNo;
        return base.InitializeAsync(navigationData);
    }

    private async Task Confirm()
    {
        var isValid = CodeValidate();
        if (isValid)
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Checking Code...");
            var response = await _usersApiService.ConfirmPhoneNo(new ConfirmPhoneRequest()
            { PhoneNo = _phoneNo, Token = Code.Value });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<AuthResult>();
                if (result.IsSuccess)
                {
                    Barrel.Current.Add("Token", result.Token, TimeSpan.FromMinutes(6));
                    Barrel.Current.Add("RefreshToken", result.RefreshToken, TimeSpan.FromDays(150));
                    BaseHttpClient.Instance.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.Token);
                    await _navigationService.NavigateToAsync<ProfilePictureViewModel>();
                }
                else
                {
                    _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                }
            }
            else
            {
                var result = await response.Content.ReadAsMessagePackAsync<AuthResult>();
                _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
            }
            await _navigationService.RemovePopupAsync();
        }
        else
        {
            _toastService.ShortAlert("کد فعالسازی را وارد کنید", MessageType.Error);
        }
    }

    private async Task Pop()
    {
        await _navigationService.RemoveLastFromBackStackAsync();
    }

    private void AddValidations()
    {
        Code.Validations.Add(new IsNotNullOrEmptyRule<string>(_localizer)
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