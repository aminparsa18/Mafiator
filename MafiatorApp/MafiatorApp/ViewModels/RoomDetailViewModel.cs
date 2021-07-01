using System;
using System.Linq;
using System.Threading.Tasks;
using MafiatorApp.Dtos;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using MafiatorApp.ViewModels.Base.Interfaces;
using MafiatorApp.Views;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
   public class RoomDetailViewModel:ViewModelBase
    {
        public ObservableRangeCollection<RoomMemberDto> Members { get; set; }
        public ObservableRangeCollection<RoomGameDto> Games { get; set; }
        private RoomDto room;
        public RoomDto Room
        {
            get => room;
            set
            {
                room = value;
                RaisePropertyChanged(()=>Room);
            }
        }
        private bool hasGame;
        public bool HasGame
        {
            get => hasGame;
            set
            {
                hasGame = value;
                RaisePropertyChanged(() => HasGame);
            }
        }
        private string time;
        public string Time
        {
            get => time;
            set
            {
                time = value;
                RaisePropertyChanged(() => Time);
            }
        }
        public IAsyncCommand LoadMembersCommand { get; set; }
        public IAsyncCommand JoinCommand { get; set; }
        public RoomDetailViewModel()
        {
            Members = new ObservableRangeCollection<RoomMemberDto>();
            Games = new ObservableRangeCollection<RoomGameDto>();
            LoadMembersCommand=new AsyncCommand(LoadMembers);
            JoinCommand=new AsyncCommand(Join);
        }

        private async Task Join()
        {
            await PopupNavigation.Instance.PushAsync(new WaitingView());
            //var response=await webApiService.JoinRoom(new RoomJoinDto(){RoomId = Room.Id});
            //if (response.IsSuccessStatusCode)
            //{
            //    var res = await response.Content.ReadAsMessagePackAsync<ApiResult>();
            //    if (res.IsSuccess)
            //    {

            //    }
            //}
            //else
            //{
            //    var err = await response.Content.ReadAsStringAsync();
            //    DependencyService.Get<IAlert>().ShortAlert(err,MessageType.Error);
            //}
        }

        private async Task LoadMembers()
        {
            var members = await WebApiService.GetMembersByRoom(Room.Id);
            if (members.IsSuccess)
            {
                Members.AddRange(members.Data);
            }
            var games = await WebApiService.GetGamesByRoom(Room.Id.ToString());
            if (games.IsSuccess)
            {
                Games.AddRange(games.Data);
                HasGame = Games.Any();
                if (HasGame)
                {
                    var totalSec = (DateTime.Now - Games[0].StartDate).TotalSeconds;
                    Device.StartTimer(TimeSpan.FromSeconds(1), () =>
                    {
                        var ts = TimeSpan.FromSeconds(totalSec);
                        Time = $"{ts.Days:00}:{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}";
                        totalSec--;
                        return totalSec != 0;
                    });
                }
            }

        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is RoomDto room)
            {
                Room = room;
                await LoadMembers();
            }else if (navigationData is Ulid roomId)
            {
                Room=new RoomDto(){Id = roomId};
                await LoadMembers();
            }
        }
    }
}
