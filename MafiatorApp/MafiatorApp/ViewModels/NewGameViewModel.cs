using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using MafiatorApp.Helpers;
using MafiatorApp.Models;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MessagePipe;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class NewGameViewModel : ViewModelBase
    {
        private ValidatableObject<short> capacity;
        public ValidatableObject<short> Capacity
        {
            get => capacity;
            set => SetProperty(ref capacity, value);
        }

        private bool isCapacityValid;

        public bool IsCapacityValid
        {
            get => isCapacityValid;
            set => SetProperty(ref isCapacityValid, value);
        }

        private bool isImmediate;

        public bool IsImmediate
        {
            get => isImmediate;
            set => SetProperty(ref isImmediate, value);
        }

        private bool isPublic;

        public bool IsPublic
        {
            get => isPublic;
            set => SetProperty(ref isPublic, value);
        }
        private DateTime? date = DateTime.Now;

        public DateTime? Date
        {
            get => date;
            set => SetProperty(ref date, value);
        }

        public ObservableRangeCollection<NewGameRole> Roles { get; set; }
        public IAsyncCommand SetRolesCommand { get; set; }
        public IAsyncCommand SaveGameCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public ICommand PublicHelpCommand { get; set; }

        private readonly IGamesApiService _gamesApiService;
        private readonly IPublisher<UpdateRoomEvent> _publisher;
        private Guid _roomId;

        public NewGameViewModel(IGamesApiService gamesApiService, IPublisher<UpdateRoomEvent> publisher)
        {
            _gamesApiService = gamesApiService;
            _publisher = publisher;
            Capacity = new ValidatableObject<short> {Value = 6};
            Roles = new ObservableRangeCollection<NewGameRole>();
            SetRolesCommand = new AsyncCommand(SetRoles);
            SaveGameCommand = new AsyncCommand(SaveGame);
            PublicHelpCommand=new Command(PublicHelp);
            PopCommand = new AsyncCommand(Pop);
        }

        private static void PublicHelp()
        {
            DependencyService.Get<IAlert>().ShortAlert("Everyone can observe your game live as guests",MessageType.Info);
        }

        private async Task Pop()
        {
            SystemConstant.SelectedRoles = null;
            await NavigationService.RemovePopupAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Guid roomId)
            {
                _roomId = roomId;
            }

            return base.InitializeAsync(navigationData);
        }

        private async Task SaveGame()
        {
            if (Capacity.Value < 6)
            {
                DependencyService.Get<IAlert>().ShortAlert("Game Players must be at least 6 person", MessageType.Error);
                return;
            }

            if (SystemConstant.SelectedRoles == null || !SystemConstant.SelectedRoles.Any())
            {
                await NavigationService.NavigateToPopupAsync<SetRolesViewModel>(Capacity.Value);
                return;
            }

            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Creating Game...");
            var response = await _gamesApiService.AddGame(new GameCreateRequest()
            {
                RoomId = _roomId,
                Roles = SystemConstant.SelectedRoles.Select(s => new GameRoleCreateRequest() {Role = s.Role, Count = s.Count})
                    .ToList(),
                StartDate = IsImmediate ? DateTime.Now : Date.Value
            });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult<GameCreateResult>>();
                if (result.IsSuccess)
                {
                    SystemConstant.SelectedRoles = null;
                    _publisher.Publish(new UpdateRoomEvent());
                    Admob.Load();
                    await NavigationService.RemovePopupAsync();
                }
                else
                {
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
                }
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                DependencyService.Get<IAlert>().ShortAlert("result.Errors[0]", MessageType.Error);
            }

            await NavigationService.RemovePopupAsync();
        }

        private async Task SetRoles()
        {
            if (Capacity.Value < 6)
            {
                DependencyService.Get<IAlert>().ShortAlert("Game Players must be at least 6 person", MessageType.Error);
            }
            else
                await NavigationService.NavigateToPopupAsync<SetRolesViewModel>(Capacity.Value);
        }

        public void RefreshRoles()
        {
            Roles.Clear();
            Roles.AddRange(SystemConstant.SelectedRoles);
        }

        public void SetTime(in TimeSpan time)
        {
            if (!Date.HasValue)
                return;
            Date = Date.Value.Date + time;
        }
    }
}