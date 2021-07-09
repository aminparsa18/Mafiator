using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Dtos;
using MafiatorApp.Enums;
using MafiatorApp.ViewModels.Base;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class PlayerRoleViewModel:ViewModelBase
    {
        private int timer;
        private double progressTimer;
        public double ProgressTimer
        {
            get => progressTimer;
            set => SetProperty(ref progressTimer, value);
        }
       
        private bool canClose;
        public bool CanClose
        {
            get => canClose;
            set => SetProperty(ref canClose, value);
        }

        private bool isMafia;
        public bool IsMafia
        {
            get => isMafia;
            set => SetProperty(ref isMafia, value);
        }

        private string gameId;
        private bool first;
        private GameRole? role;

        public GameRole? Role
        {
            get=>role;
            set => SetProperty(ref role, value);
        }

        private bool fromDetail;
        public IAsyncCommand PopCommand { get; set; }
        public ObservableRangeCollection<PartnerDto> Partners { get; set; }
        public PlayerRoleViewModel()
        {
            Partners=new ObservableRangeCollection<PartnerDto>();
            PopCommand=new AsyncCommand(Pop);
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

        private async Task LoadRole()
        {
            if (first)
                Device.StartTimer(TimeSpan.FromMilliseconds(100), () =>
                {
                    timer += 100;
                    ProgressTimer = 100 * (double)timer / 40000;
                    if (timer == 40000)
                    {
                        CanClose = true;
                        return false;
                    }
                    return true;
                });
            else
                CanClose = true;
            
            if(fromDetail)return;
            IsMafia = Role == GameRole.Mafia || Role == GameRole.GodFather;
                if (IsMafia)
                {
                    var partners = await WebApiService.GetMafiaPartners(gameId);
                    if (partners.IsSuccess)
                    {
                        var members = Barrel.Current.Get<IEnumerable<PlayerDto>>("Members");
                        Partners.AddRange(partners.Data.Select(s=>new PartnerDto()
                        {
                            Role = s.Role,
                            Name = members.FirstOrDefault(m=>m.Id==s.MemberId)?.DisplayName
                        }));
                    }
                }
         
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is Tuple<GameRole?, string,bool> data)
            {
                this.Role = data.Item1;
                this.gameId = data.Item2;
                this.first = data.Item3;
                await LoadRole();
            }else if (navigationData is Tuple<GameRole?, bool> data2)
            {
                this.Role = data2.Item1;
                this.fromDetail = true;
                await LoadRole();
            }
        }

        
    }
}
