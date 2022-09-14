using Mafiator.Common.Client;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using MafiatorApp.Models;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ISubscriber<Country> _subscriber;
        private readonly IUsersApiService _usersApiService;

        private ValidatableObject<string> _loginUsername;
        
        public ValidatableObject<string> LoginUsername
        {
            get => _loginUsername;
            set => SetProperty(ref _loginUsername, value);
        }

        private ValidatableObject<string> _loginPassword;

        public ValidatableObject<string> LoginPassword
        {
            get => _loginPassword;
            set => SetProperty(ref _loginPassword, value);
        }

        private ValidatableObject<string> _phoneNo;
        public ValidatableObject<string> PhoneNo
        {
            get => _phoneNo;
            set => SetProperty(ref _phoneNo, value);
        }

        private ValidatableObject<string> _username;
        public ValidatableObject<string> Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private ValidatableObject<string> _confirmPassword;
        public ValidatableObject<string> ConfirmPassword
        {
            get => _confirmPassword;
            set => SetProperty(ref _confirmPassword, value);
        }

        private bool _isLoginPhoneNoValid = true;
        public bool IsLoginPhoneNoValid
        {
            get => _isLoginPhoneNoValid;
            set => SetProperty(ref _isLoginPhoneNoValid, value);
        }

        private bool _isLoginPasswordValid = true;
        public bool IsLoginPasswordValid
        {
            get => _isLoginPasswordValid;
            set => SetProperty(ref _isLoginPasswordValid, value);
        }

        private bool _isPhoneNoValid = true;
        public bool IsPhoneNoValid
        {
            get => _isPhoneNoValid;
            set => SetProperty(ref _isPhoneNoValid, value);
        }

        private bool _isPasswordValid = true;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set => SetProperty(ref _isPasswordValid, value);
        }

        private bool _isConfirmPasswordValid = true;
        public bool IsConfirmPasswordValid
        {
            get => _isConfirmPasswordValid;
            set => SetProperty(ref _isConfirmPasswordValid, value);
        }

        private bool _isUsernameValid = true;
        public bool IsUsernameValid
        {
            get => _isUsernameValid;
            set => SetProperty(ref _isUsernameValid, value);
        }

        private Country country=new()
        {
            Name = "United States",
            Code = "US",
            DialCode = "+1"
        };

        public Country Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }
        public IAsyncCommand LoginCommand { get; set; }
        public IAsyncCommand RegisterCommand { get; set; }
        public IAsyncCommand GoogleCommand { get; set; }
        public IAsyncCommand FacebookCommand { get; set; }
        public IAsyncCommand ShowCountriesCommand { get; set; }

        public LoginViewModel(ISubscriber<Country> subscriber, IUsersApiService usersApiService)
        {
            _subscriber = subscriber;
            _usersApiService = usersApiService;
            LoginCommand = new AsyncCommand(Login);
            RegisterCommand = new AsyncCommand(Register);
            GoogleCommand = new AsyncCommand(Google);
            FacebookCommand = new AsyncCommand(Facebook);
            ShowCountriesCommand = new AsyncCommand(ShowCountries);
            LoginUsername = new ValidatableObject<string>();
            LoginPassword = new ValidatableObject<string>();
            Username = new ValidatableObject<string>();
            PhoneNo = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            ConfirmPassword = new ValidatableObject<string>();
            AddValidations();
            subscriber.Subscribe(c => Country = c);
        }

        private async Task ShowCountries()
        {
            await NavigationService.NavigateToPopupAsync<CountriesViewModel>();
        }

        private static async Task Google()
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
            LoginUsername.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = LocalizationResourceManager.Current.GetValue("EmptyUsername") });
            //LoginPhoneNo.Validations.Add(new PhoneNoRule<string>());
            LoginPassword.Validations.Add(new IsNotNullOrEmptyRule<string>()
            { ValidationMessage = LocalizationResourceManager.Current.GetValue("EmptyPassword") });
            PhoneNo.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = LocalizationResourceManager.Current.GetValue("EmptyPhoneNo") });
            Username.Validations.Add(new IsNotNullOrEmptyRule<string>
            { ValidationMessage = LocalizationResourceManager.Current.GetValue("EmptyUsername") });
            Password.Validations.Add(new IsNotNullOrEmptyRule<string>()
            { ValidationMessage = LocalizationResourceManager.Current.GetValue("EmptyPassword") });
            ;
            Password.Validations.Add(new PasswordRule<string>());
            ConfirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string>()
            { ValidationMessage = LocalizationResourceManager.Current.GetValue("EmptyConfirmPassword") });
            ;
            ConfirmPassword.Validations.Add(new PasswordRule<string>());
        }

        private async Task Login()
        {
            var isValid = ValidateLogin();
            if (isValid)
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Logging in...");
                try
                {
                    var response = await _usersApiService.Login(new UserLoginRequest()
                    {
                        Password = LoginPassword.Value,
                        Username = LoginUsername.Value
                    });

                    if (response.IsSuccessStatusCode)
                    {
                        var result = await response.Content.ReadAsMessagePackAsync<AuthResult>();
                        if (result.IsSuccess)
                        {
                            Barrel.Current.Add("Token", result.Token, TimeSpan.FromMinutes(20));
                            Barrel.Current.Add("RefreshToken", result.RefreshToken, TimeSpan.FromDays(150));
                            BaseHttpClient.Instance.DefaultRequestHeaders.Authorization =
                                new AuthenticationHeaderValue("Bearer", result.Token);
                            await NavigationService.NavigateToAsync<HomeViewModel>();
                        }
                        else if (result.StatusCode == ApiResultStatusCode.Forbidden)
                        {
                           await NavigationService.NavigateToAsync<ConfirmPhoneViewModel>(result.Token);
                            DependencyService.Get<IAlert>()
                                .ShortAlert(string.Join(",", result.Errors), MessageType.Error);
                        }
                        else
                        {
                            DependencyService.Get<IAlert>()
                                .ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                        }
                    }
                    else
                    {
                        var reason = await response.Content.ReadAsStringAsync();
                        DependencyService.Get<IAlert>()
                            .ShortAlert(reason, MessageType.Error);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex, new Dictionary<string, string>()
                    {
                        {"Task", nameof(Login)},
                        {"Sender", nameof(LoginViewModel)}
                    });
                    DependencyService.Get<IAlert>().ShortAlert("OOPS!!Server error occured. Please try again later",
                        MessageType.Error);
                }

                await NavigationService.RemovePopupAsync();
            }
        }

        private async Task Register()
        {
            var isValid = ValidateRegister();
            if (isValid)
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Registering your account");
                try
                {
                    var response = await _usersApiService.RegisterUser(new RegisterUserRequest()
                    {
                        Username = Username.Value,
                        PhoneNumber = Country.DialCode+PhoneNo.Value,
                        Password = Password.Value,
                        CountryCode = Country.Code
                    });
                    var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
                    if (response.IsSuccessStatusCode)
                    {
                        if (result.IsSuccess)
                        {
                            await NavigationService.NavigateToAsync<ConfirmPhoneViewModel>(Country.DialCode + PhoneNo.Value);
                        }
                        else
                        {
                            DependencyService.Get<IAlert>()
                                .ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                        }
                    }
                    else
                    {
                        DependencyService.Get<IAlert>()
                            .ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                    }
                }
                catch (Exception ex)
                {
                    Crashes.TrackError(ex, new Dictionary<string, string>()
                    {
                        {"Task", nameof(Register)},
                        {"Sender", nameof(LoginViewModel)}
                    });
                    DependencyService.Get<IAlert>().ShortAlert("OOPS!!Server error occured. Please try again later",
                        MessageType.Error);
                }

                await NavigationService.RemovePopupAsync();
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
                DependencyService.Get<IAlert>()
                    .ShortAlert(LocalizationResourceManager.Current.GetValue("PasswordNotMatch"), MessageType.Error);
            IsPhoneNoValid = PhoneNo.Validate();
            IsUsernameValid = Username.Validate();
            IsPasswordValid = Password.Validate();
            IsConfirmPasswordValid = ConfirmPassword.Validate();
            return IsPhoneNoValid && IsPasswordValid && IsConfirmPasswordValid;
        }
    }
}