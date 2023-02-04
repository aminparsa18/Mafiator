using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Countries;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using MessagePipe;
using Microsoft.AppCenter.Crashes;
using System.Net.Http.Headers;

namespace Mafiator.Game.ViewModels;

public partial class LoginViewModel : ViewModelBase
{
    private readonly ISubscriber<CountryResult> _subscriber;
    private readonly IUsersApiService _usersApiService;

    [ObservableProperty]
    private string _kir;

    [ObservableProperty]
    private ValidatableObject<string> _loginUsername;

    [ObservableProperty]
    private ValidatableObject<string> _loginPassword;

    [ObservableProperty]
    private ValidatableObject<string> _phoneNo;

    [ObservableProperty]
    private ValidatableObject<string> _username;

    [ObservableProperty]
    private ValidatableObject<string> _password;

    [ObservableProperty]
    private ValidatableObject<string> _confirmPassword;

    [ObservableProperty]
    private bool _isLoginPhoneNoValid = true;

    [ObservableProperty]
    private bool _isLoginPasswordValid = true;

    [ObservableProperty]
    private bool _isPhoneNoValid = true;

    [ObservableProperty]
    private bool _isPasswordValid = true;

    [ObservableProperty]
    private bool _isConfirmPasswordValid = true;

    [ObservableProperty]
    private bool _isUsernameValid = true;

    [ObservableProperty]
    private CountryResult _country = new()
    {
        Name = "United States",
        Sign = "US",
        Code = "+1"
    };
    
    public LoginViewModel(INavigationService navigationService, IToastService toastService, ISubscriber<CountryResult> subscriber, IUsersApiService usersApiService) 
        : base(navigationService, toastService)
    {
        _subscriber = subscriber;
        _usersApiService = usersApiService;
        LoginUsername = new ValidatableObject<string>();
        LoginPassword = new ValidatableObject<string>();
        Username = new ValidatableObject<string>();
        PhoneNo = new ValidatableObject<string>();
        Password = new ValidatableObject<string>();
        ConfirmPassword = new ValidatableObject<string>();
        AddValidations();
        subscriber.Subscribe(c => Country = c);
    }

    [RelayCommand]
    private async Task ShowCountries() => await _navigationService.NavigateToPopupAsync<CountriesViewModel>();

    [RelayCommand]
    private async Task Google()
    {
        try
        {
            var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri("https://mafiator.com/home/googlelogin"),
                new Uri("app://callback.mftor"));

        }
        catch (TaskCanceledException)
        {
        }
    }

    [RelayCommand]
    private static async Task Facebook()
    {
        try
        {
            var authResult = await WebAuthenticator.AuthenticateAsync(
                new Uri("https://mafiator.com/home/facebooklogin"),
                new Uri("app://callback.mftor"));

        }
        catch (TaskCanceledException)
        {
        }
    }

    private void AddValidations()
    {
        LoginUsername.Validations.Add(new IsNotNullOrEmptyRule<string>()
        { ValidationMessage = LocalizationResourceManager.Instance["EmptyUsername"] });
        //LoginPhoneNo.Validations.Add(new PhoneNoRule<string>());
        LoginPassword.Validations.Add(new IsNotNullOrEmptyRule<string>()
        { ValidationMessage = LocalizationResourceManager.Instance["EmptyPassword"] });
        PhoneNo.Validations.Add(new IsNotNullOrEmptyRule<string>()
        { ValidationMessage = LocalizationResourceManager.Instance["EmptyPhoneNo"] });
        Username.Validations.Add(new IsNotNullOrEmptyRule<string>()
        { ValidationMessage = LocalizationResourceManager.Instance["EmptyUsername"] });
        Password.Validations.Add(new IsNotNullOrEmptyRule<string>()
        { ValidationMessage = LocalizationResourceManager.Instance["EmptyPassword"] });
        Password.Validations.Add(new PasswordRule<string>());
        ConfirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string>()
        { ValidationMessage = LocalizationResourceManager.Instance["EmptyConfirmPassword"] });
        ConfirmPassword.Validations.Add(new PasswordRule<string>());
    }

    [RelayCommand]
    private async Task Login()
    {
        var isValid = ValidateLogin();
        if (isValid)
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Instance["LoggingIn"]);
            try
            {
                var response = await _usersApiService.Login(new UserLoginRequest()
                {
                    Password = LoginPassword.Value,
                    Username = LoginUsername.Value
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsMemoryPackAsync<AuthResult>();
                    if (result.IsSuccess)
                    {
                        Barrel.Current.Add("Token", result.Token, TimeSpan.FromMinutes(20));
                        Barrel.Current.Add("RefreshToken", result.RefreshToken, TimeSpan.FromDays(150));
                        BaseHttpClient.Instance.DefaultRequestHeaders.Authorization =
                            new AuthenticationHeaderValue("Bearer", result.Token);
                        await _navigationService.NavigateToAsync(nameof(HomeViewModel));
                    }
                    else if (result.StatusCode == ApiResultStatusCode.Forbidden)
                    {
                        await _navigationService.NavigateToAsync($"{nameof(ConfirmPhoneViewModel)}?phoneNo={result.Token}");
                        _toastService.ShortAlert(string.Join(",", result.Errors), MessageType.Error);
                    }
                    else
                    {
                        _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                    }
                }
                else
                {
                    var reason = await response.Content.ReadAsStringAsync();
                    _toastService.ShortAlert(reason, MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>()
                {
                    {"Task", nameof(Login)},
                    {"Sender", nameof(LoginViewModel)}
                });
                _toastService.ShortAlert(LocalizationResourceManager.Instance["ServerError"], MessageType.Error);
            }

            await _navigationService.RemovePopupAsync();
        }
    }

    [RelayCommand]
    private async Task Register()
    {
        var isValid = ValidateRegister();
        if (isValid)
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>(LocalizationResourceManager.Instance["RegisteringAccount"]);
            try
            {
                var response = await _usersApiService.RegisterUser(new RegisterUserRequest()
                {
                    Username = Username.Value,
                    PhoneNumber = Country.Code + PhoneNo.Value,
                    Password = Password.Value,
                    CountryCode = Country.Sign
                });
                var ss = await response.Content.ReadAsStringAsync();
                var result = await response.Content.ReadAsMemoryPackAsync<ApiResult>();
                if (response.IsSuccessStatusCode)
                {
                    if (result.IsSuccess)
                        await _navigationService.NavigateToAsync($"{nameof(ConfirmPhoneViewModel)}?phoneNo={Uri.EscapeDataString(Country.Code + PhoneNo.Value)}");
                    else
                    {
                        _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                    }
                }
                else
                {
                    _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex, new Dictionary<string, string>()
                {
                    {"Task", nameof(Register)},
                    {"Sender", nameof(LoginViewModel)}
                });
                _toastService.ShortAlert(LocalizationResourceManager.Instance["ServerError"], MessageType.Error);
            }

            await _navigationService.RemovePopupAsync();
        }
    }

    private bool ValidateLogin()
    {
        IsLoginPhoneNoValid = LoginUsername.Validate();
        IsLoginPasswordValid = LoginPassword.Validate();
        return IsLoginPhoneNoValid && IsLoginPasswordValid;
    }

    private bool ValidateRegister()
    {
        if (Password.Value != ConfirmPassword.Value)
            _toastService.ShortAlert(LocalizationResourceManager.Instance["PasswordNotMatch"], MessageType.Error);
        IsPhoneNoValid = PhoneNo.Validate();
        IsUsernameValid = Username.Validate();
        IsPasswordValid = Password.Validate();
        IsConfirmPasswordValid = ConfirmPassword.Validate();
        return IsPhoneNoValid && IsPasswordValid && IsConfirmPasswordValid;
    }
}