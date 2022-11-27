using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Rooms;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;
using Plugin.MauiMTAdmob;
using System.Windows.Input;

namespace Mafiator.Game.ViewModels
{
    public class NewRoomViewModel : ViewModelBase
    {
        private ValidatableObject<string> _name;

        public ValidatableObject<string> Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private bool _isPrivate;

        public bool IsPrivate
        {
            get => _isPrivate;
            set => SetProperty(ref _isPrivate, value);
        }

        private bool _isNameValid;

        public bool IsNameValid
        {
            get => _isNameValid;
            set => SetProperty(ref _isNameValid, value);
        }

        private Country country;

        public Country Country
        {
            get => country;
            set => SetProperty(ref country, value);
        }

        public IAsyncRelayCommand AddRoomCommand { get; set; }
        public IAsyncRelayCommand AddByCodeCommand { get; set; }
        public IAsyncRelayCommand PopCommand { get; set; }
        public IRelayCommand PrivateHelpCommand { get; set; }

        private readonly IRoomsApiService _roomsApiService;
        private List<Country> countries;

        public NewRoomViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
            IRoomsApiService roomsApiService) : base(navigationService, localizer, toastService)
        {
            _roomsApiService = roomsApiService;
            Name = new ValidatableObject<string>();
            AddValidations();
            AddRoomCommand = new AsyncRelayCommand(AddRoom);
            AddByCodeCommand = new AsyncRelayCommand(AddByCode);
            PopCommand = new AsyncRelayCommand(Pop);
            PrivateHelpCommand = new RelayCommand(PrivateHelp);
            LoadData();
        }

        private void PrivateHelp()
        {
            _toastService.ShortAlert("Members can only join using invitation link", MessageType.Info);
        }

        private void LoadData()
        {
            Task.Run(() =>
            {
                countries = Barrel.Current.Get<List<Country>>("Countries");
                var code = Barrel.Current.Get<UserDetailsResult>("User")?.CountryCode;
                Country = countries.FirstOrDefault(c => c.Code == code);
            });
        }

        private async Task AddByCode()
        {
            await _navigationService.NavigateToPopupAsync<NewMemberViewModel>();
        }

        private async Task Pop()
        {
            await _navigationService.RemovePopupAsync();
        }

        private async Task AddRoom()
        {
            var isValid = Validate();
            if (isValid)
            {
                await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Creating Room...");
                var response = await _roomsApiService.AddRoom(new RoomCreateRequest()
                {
                    Name = Name.Value,
                    IsPrivate = IsPrivate,
                    Users = SystemConstant.Members.Select(s => s.Id).ToList(),
                    Country = Country.Code
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsMessagePackAsync<ApiResult<RoomCreateResult>>();
                    if (result.IsSuccess)
                    {
                        SystemConstant.Members = null;
                        await _navigationService.RemovePopupAsync();
                        await _navigationService.RemovePopupAsync();
                        await _navigationService.NavigateToAsync<RoomDetailViewModel>(result.Data.Id);
                        CrossMauiMTAdmob.Current.LoadInterstitial("");
                    }
                    else
                    {
                        await _navigationService.RemovePopupAsync();
                        _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                    }
                }
                else
                {
                    await _navigationService.RemovePopupAsync();
                    var content = await response.Content.ReadAsStringAsync();
                    _toastService.ShortAlert(content, MessageType.Error);
                }
            }
        }

        private void AddValidations()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule<string>(_localizer) { ValidationMessage = "Enter room name" });
        }

        private bool Validate()
        {
            IsNameValid = Name.Validate();
            return IsNameValid;
        }
    }
}