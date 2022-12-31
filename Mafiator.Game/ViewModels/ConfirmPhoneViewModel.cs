using CommunityToolkit.Mvvm.ComponentModel;
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
using System.Net.Http.Headers;

namespace Mafiator.Game.ViewModels;

[QueryProperty(nameof(PhoneNo), "phoneNo")]
public partial class ConfirmPhoneViewModel : ViewModelBase
{
    private readonly IUsersApiService _usersApiService;

    [ObservableProperty]
    private string _phoneNo;

    [ObservableProperty]
    private ValidatableObject<string> _code;

    [ObservableProperty]
    private bool _isCodeValid;

    public IAsyncRelayCommand ConfirmCommand { get; set; }

    public ConfirmPhoneViewModel(INavigationService navigationService, IToastService toastService, IUsersApiService usersApiService) 
        : base(navigationService, toastService)
    {
        _usersApiService = usersApiService;
        ConfirmCommand = new AsyncRelayCommand(Confirm);
        Code = new ValidatableObject<string>();
        AddValidations();
        IsCodeValid = true;
    }

    private async Task Confirm()
    {
        var isValid = CodeValidate();
        if (isValid)
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Instance["ValidatingCode"]);
            var response = await _usersApiService.ConfirmPhoneNo(new ConfirmPhoneRequest()
            { PhoneNo = _phoneNo, Token = Code.Value });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMemoryPackAsync<AuthResult>();
                if (result.IsSuccess)
                {
                    Barrel.Current.Add("Token", result.Token, TimeSpan.FromMinutes(6));
                    Barrel.Current.Add("RefreshToken", result.RefreshToken, TimeSpan.FromDays(150));
                    BaseHttpClient.Instance.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", result.Token);
                    await _navigationService.NavigateToAsync(nameof(ProfilePictureViewModel));
                }
                else
                {
                    _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                }
            }
            else
            {
                var result = await response.Content.ReadAsMemoryPackAsync<AuthResult>();
                _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
            }
            await _navigationService.RemovePopupAsync();
        }
    }

    private void AddValidations()
    {
        Code.Validations.Add(new IsNotNullOrEmptyRule<string>()
        {
            ValidationMessage = LocalizationResourceManager.Instance["EmptyCode"]
        });
    }

    private bool CodeValidate()
    {
        IsCodeValid = Code.Validate();
        return IsCodeValid;
    }
}