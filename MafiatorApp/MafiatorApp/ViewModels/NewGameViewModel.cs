using System;
using System.Linq;
using MafiatorApp.Validations;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Extentions;
using MafiatorApp.Models;
using MafiatorApp.Models.Api;
using MafiatorApp.Models.PipeEvents;
using MafiatorApp.Services;
using MafiatorApp.Views;
using MessagePipe;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class NewGameViewModel : ViewModelBase
    {
        private readonly IPublisher<UpdateRoomEvent> publisher;
        private ValidatableObject<short> capacity;
        public ValidatableObject<short> Capacity
        {
            get => capacity;
            set
            {
                capacity = value;
                RaisePropertyChanged(() => Capacity);
            }
        }

        private bool isCapacityValid;

        public bool IsCapacityValid
        {
            get => isCapacityValid;
            set
            {
                isCapacityValid = value;
                RaisePropertyChanged(() => IsCapacityValid);
            }
        }

        private bool isImmediate;

        public bool IsImmediate
        {
            get => isImmediate;
            set
            {
                isImmediate = value;
                RaisePropertyChanged(() => IsImmediate);
            }
        }

        private DateTime? date = DateTime.Now;

        public DateTime? Date
        {
            get => date;
            set
            {
                date = value;
                RaisePropertyChanged(() => Date);
            }
        }

        public ObservableRangeCollection<NewGameRole> Roles { get; set; }
        public IAsyncCommand SetRolesCommand { get; set; }
        public IAsyncCommand SaveGameCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        private Ulid _roomId;
        public NewGameViewModel(IPublisher<UpdateRoomEvent> publisher)
        {
            this.publisher = publisher;
            Capacity = new ValidatableObject<short> {Value = 6};
            Roles = new ObservableRangeCollection<NewGameRole>();
            SetRolesCommand = new AsyncCommand(SetRoles);
            SaveGameCommand = new AsyncCommand(SaveGame);
            PopCommand = new AsyncCommand(Pop);
        }

        private async Task Pop()
        {
            SystemConstant.SelectedRoles = null;
            await NavigationService.RemovePopupAsync();
        }

        public override Task InitializeAsync(object navigationData)
        {
            if (navigationData is Ulid roomId)
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
            var response = await WebApiService.AddGame(new GameCreateDto()
            {
                RoomId = _roomId,
                Roles = SystemConstant.SelectedRoles.Select(s => new GameRoleDto() {Role = s.Role, Count = s.Count})
                    .ToList(),
                StartDate = IsImmediate ? DateTime.Now : Date.Value
            });
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult<GameResultDto>>();
                if (result.IsSuccess)
                {
                    SystemConstant.SelectedRoles = null;
                    //MessagingCenter.Send(this, "GameCreated");
                    publisher.Publish(new UpdateRoomEvent());
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