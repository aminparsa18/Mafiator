using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Models;
using MafiatorApp.Models.Api;
using MafiatorApp.Resources.Texts;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using MessagePipe;
using Microsoft.AppCenter.Crashes;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace MafiatorApp.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private readonly ISubscriber<Country> subscriber;
        private ValidatableObject<string> _loginUsername;
        public ValidatableObject<string> LoginUsername
        {
            get => _loginUsername;
            set
            {
                _loginUsername = value;
                RaisePropertyChanged(() => LoginUsername);
            }
        }

        private ValidatableObject<string> _loginPassword;
        public ValidatableObject<string> LoginPassword
        {
            get => _loginPassword;
            set
            {
                _loginPassword = value;
                RaisePropertyChanged(() => LoginPassword);
            }
        }

        private ValidatableObject<string> _phoneNo;
        public ValidatableObject<string> PhoneNo
        {
            get => _phoneNo;
            set
            {
                _phoneNo = value;
                RaisePropertyChanged(() => PhoneNo);
            }
        }

        private ValidatableObject<string> _displayName;
        public ValidatableObject<string> DisplayName
        {
            get => _displayName;
            set
            {
                _displayName = value;
                RaisePropertyChanged(() => DisplayName);
            }
        }

        private ValidatableObject<string> _username;
        public ValidatableObject<string> Username
        {
            get => _username;
            set
            {
                _username = value;
                RaisePropertyChanged(() => Username);
            }
        }

        private ValidatableObject<string> _password;
        public ValidatableObject<string> Password
        {
            get => _password;
            set
            {
                _password = value;
                RaisePropertyChanged(() => Password);
            }
        }

        private ValidatableObject<string> _confirmPassword;
        public ValidatableObject<string> ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                RaisePropertyChanged(() => ConfirmPassword);
            }
        }

        private bool _isLoginPhoneNoValid = true;
        public bool IsLoginPhoneNoValid
        {
            get => _isLoginPhoneNoValid;
            set
            {
                _isLoginPhoneNoValid = value;
                RaisePropertyChanged(() => IsLoginPhoneNoValid);
            }
        }

        private bool _isLoginPasswordValid = true;
        public bool IsLoginPasswordValid
        {
            get => _isLoginPasswordValid;
            set
            {
                _isLoginPasswordValid = value;
                RaisePropertyChanged(() => IsLoginPasswordValid);
            }
        }

        private bool _isPhoneNoValid = true;
        public bool IsPhoneNoValid
        {
            get => _isPhoneNoValid;
            set
            {
                _isPhoneNoValid = value;
                RaisePropertyChanged(() => IsPhoneNoValid);
            }
        }

        private bool _isPasswordValid = true;
        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            set
            {
                _isPasswordValid = value;
                RaisePropertyChanged(() => IsPasswordValid);
            }
        }

        private bool _isConfirmPasswordValid = true;
        public bool IsConfirmPasswordValid
        {
            get => _isConfirmPasswordValid;
            set
            {
                _isConfirmPasswordValid = value;
                RaisePropertyChanged(() => IsConfirmPasswordValid);
            }
        }

        private bool _isDisplayNameValid = true;
        public bool IsDisplayNameValid
        {
            get => _isDisplayNameValid;
            set
            {
                _isDisplayNameValid = value;
                RaisePropertyChanged(() => IsDisplayNameValid);
            }
        }

        private bool _isUsernameValid = true;
        public bool IsUsernameValid
        {
            get => _isUsernameValid;
            set
            {
                _isUsernameValid = value;
                RaisePropertyChanged(() => IsUsernameValid);
            }
        }

        private Country country=new Country()
        {
            Name = "United States",
            Code = "US",
            DialCode = "+1"
        };
        public Country Country
        {
            get => country;
            set
            {
                country = value;
                RaisePropertyChanged(() => Country);
            }
        }
        public IAsyncCommand LoginCommand { get; set; }
        public IAsyncCommand RegisterCommand { get; set; }
        public IAsyncCommand GoogleCommand { get; set; }
        public IAsyncCommand FacebookCommand { get; set; }
        public IAsyncCommand ShowCountriesCommand { get; set; }

        public LoginViewModel(ISubscriber<Country> subscriber)
        {
            this.subscriber = subscriber;
            LoginCommand = new AsyncCommand(Login);
            RegisterCommand = new AsyncCommand(Register);
            GoogleCommand = new AsyncCommand(Google);
            FacebookCommand = new AsyncCommand(Facebook);
            ShowCountriesCommand = new AsyncCommand(ShowCountries);
            LoginUsername = new ValidatableObject<string>();
            LoginPassword = new ValidatableObject<string>();
            Username = new ValidatableObject<string>();
            PhoneNo = new ValidatableObject<string>();
            DisplayName = new ValidatableObject<string>();
            Password = new ValidatableObject<string>();
            ConfirmPassword = new ValidatableObject<string>();
            AddValidations();
            subscriber.Subscribe(c => Country = c);
        }

        private async Task ShowCountries()
        {
            await NavigationService.NavigateToPopupAsync<CountriesViewModel>();
        }

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

        private async Task Facebook()
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
                {ValidationMessage = TextsTranslateManager.Translate("EmptyUsername")});
            //LoginPhoneNo.Validations.Add(new PhoneNoRule<string>());
            LoginPassword.Validations.Add(new IsNotNullOrEmptyRule<string>()
                {ValidationMessage = TextsTranslateManager.Translate("EmptyPassword")});
            PhoneNo.Validations.Add(new IsNotNullOrEmptyRule<string>
                {ValidationMessage = TextsTranslateManager.Translate("EmptyPhoneNo")});
            Username.Validations.Add(new IsNotNullOrEmptyRule<string>
                { ValidationMessage = TextsTranslateManager.Translate("EmptyUsername") });
            DisplayName.Validations.Add(new IsNotNullOrEmptyRule<string>());
            Password.Validations.Add(new IsNotNullOrEmptyRule<string>()
                {ValidationMessage = TextsTranslateManager.Translate("EmptyPassword")});
            ;
            Password.Validations.Add(new PasswordRule<string>());
            ConfirmPassword.Validations.Add(new IsNotNullOrEmptyRule<string>()
                {ValidationMessage = TextsTranslateManager.Translate("EmptyPassword")});
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
                    var response = await WebApiService.Login(new UserLoginDto()
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
                    var response = await WebApiService.RegisterUser(new RegisterUserDto()
                    {
                        Username = Username.Value,
                        PhoneNumber = Country.DialCode+PhoneNo.Value,
                        DisplayName = DisplayName.Value,
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
                    .ShortAlert(TextsTranslateManager.Translate("PasswordNotMatch"), MessageType.Error);
            IsPhoneNoValid = PhoneNo.Validate();
            IsUsernameValid = Username.Validate();
            IsDisplayNameValid = DisplayName.Validate();
            IsPasswordValid = Password.Validate();
            IsConfirmPasswordValid = ConfirmPassword.Validate();
            return IsPhoneNoValid && IsDisplayNameValid && IsPasswordValid && IsConfirmPasswordValid;
        }
    }
}