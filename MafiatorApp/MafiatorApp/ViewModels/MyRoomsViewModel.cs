using MafiatorApp.Dtos.Room;
using MafiatorApp.Extensions;
using MafiatorApp.Models.Api;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class MyRoomsViewModel : ViewModelBase
    {
        private LayoutState currentState;
        public LayoutState CurrentState
        {
            get => currentState;
            set => SetProperty(ref currentState, value);
        }

        private RoomDto room;

        public RoomDto Room
        {
            get => room;
            set => SetProperty(ref room, value);
        }

        public ObservableRangeCollection<RoomDto> Rooms { get; set; }
        public IAsyncCommand LoadRoomsCommand { get; set; }
        public IAsyncCommand RoomSelectedCommand { get; set; }
        public IAsyncCommand AddRoomCommand { get; set; }

        public MyRoomsViewModel()
        {
            Rooms = new ObservableRangeCollection<RoomDto>();
            LoadRoomsCommand = new AsyncCommand(LoadRooms);
            LoadRoomsCommand.ExecuteAsync();
            RoomSelectedCommand = new AsyncCommand(RoomSelected);
            AddRoomCommand = new AsyncCommand(AddRoom);
        }

        private async Task AddRoom()
        {
            await NavigationService.NavigateToPopupAsync<NewRoomViewModel>();
        }

        private async Task RoomSelected()
        {
            if (Room == null)
                return;
            await NavigationService.NavigateToAsync<RoomDetailViewModel>(Room);
            Room = null;
        }

        private async Task LoadRooms()
        {
            CurrentState = LayoutState.Loading;
            IsBusy = true;
            var rooms = await WebApiService.GetMyRooms();
            if (rooms.IsSuccess)
            {
                Rooms.Clear();
                Rooms.AddRange(rooms.Data);
                CurrentState = !Rooms.Any() ? LayoutState.Empty : LayoutState.Success;
            }
            else
            {
                CurrentState = LayoutState.Error;
                DependencyService.Get<IAlert>().ShortAlert(rooms.Errors.ToString(), MessageType.Error);
            }

            IsBusy = false;
        }

        public async Task Leave(string code)
        {
            await NavigationService.NavigateToPopupAsync<WaitingViewModel>("Leaving...");
            var response = await WebApiService.LeaveRoom(code);
            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadAsMessagePackAsync<ApiResult>();
                if (result.IsSuccess)
                {
                    await LoadRoomsCommand.ExecuteAsync();
                }
                else
                {
                    DependencyService.Get<IAlert>().ShortAlert(result.Errors.ToString(), MessageType.Error);
                }
            }
            else
            {
                var result = await response.Content.ReadAsStringAsync();
                DependencyService.Get<IAlert>().ShortAlert(result, MessageType.Error);
            }

            await NavigationService.RemovePopupAsync();
        }
    }
}