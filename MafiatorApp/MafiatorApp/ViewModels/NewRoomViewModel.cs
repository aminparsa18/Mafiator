using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Helpers;
using MafiatorApp.Models;
using MafiatorApp.Models.Api;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
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

        public IAsyncCommand AddRoomCommand { get; set; }
        public IAsyncCommand AddByCodeCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public ICommand PrivateHelpCommand { get; set; }
        private List<Country> countries;
        public NewRoomViewModel()
        { 
            Name = new ValidatableObject<string>();
            AddValidations();
            AddRoomCommand = new AsyncCommand(AddRoom);
            AddByCodeCommand = new AsyncCommand(AddByCode);
            PopCommand = new AsyncCommand(Pop);
            PrivateHelpCommand=new Command(PrivateHelp);
           LoadData();
        }

        private void PrivateHelp()
        {
            DependencyService.Get<IAlert>().ShortAlert("Members can only join using invitation link",MessageType.Info);
        }

        private void LoadData()
        {
            Task.Run(() =>
            {
                countries = Barrel.Current.Get<List<Country>>("Countries");
                var code = Barrel.Current.Get<UserDto>("User")?.CountryCode;
                Country = countries.FirstOrDefault(c => c.Code == code);
            });
        }

        private async Task AddByCode()
        {
            await NavigationService.NavigateToPopupAsync<NewMemberViewModel>();
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

        private async Task AddRoom()
        {
            var isValid = Validate();
            if (isValid)
            {
                await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Creating Room...");
                var response = await WebApiService.AddRoom(new RoomCreateDto()
                {
                    Name = Name.Value,
                    IsPrivate = IsPrivate,
                    Users = SystemConstant.Members.Select(s => s.Id).ToList(),
                    Country = Country.Code
                });

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadAsMessagePackAsync<ApiResult<RoomIdDto>>();
                    if (result.IsSuccess)
                    {
                        SystemConstant.Members = null;
                        await NavigationService.RemovePopupAsync();
                        await NavigationService.RemovePopupAsync();
                        await NavigationService.NavigateToAsync<RoomDetailViewModel>(result.Data.Id);
                        Admob.Load();
                    }
                    else
                    {
                        await NavigationService.RemovePopupAsync();
                        DependencyService.Get<IAlert>()
                            .ShortAlert(string.Join(',', result.Errors), MessageType.Error);
                    }
                }
                else
                {
                    await NavigationService.RemovePopupAsync();
                    var content = await response.Content.ReadAsStringAsync();
                    DependencyService.Get<IAlert>()
                        .ShortAlert(content, MessageType.Error);
                }
            }
        }

        private void AddValidations()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> {ValidationMessage = "Enter room name"});
        }

        private bool Validate()
        {
            IsNameValid = Name.Validate();
            return IsNameValid;
        }
    }
}