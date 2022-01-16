using MafiatorApp.Enums;
using MafiatorApp.Models;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class SetRolesViewModel : ViewModelBase
    {
        private int selectedCount;

        public int SelectedCount
        {
            get => selectedCount;
            set => SetProperty(ref selectedCount, value);
        }

        private short totalCount;

        public short TotalCount
        {
            get => totalCount;
            set => SetProperty(ref totalCount, value);
        }

        public ObservableRangeCollection<NewGameRole> Roles { get; set; }
        private NewGameRole role;

        public NewGameRole Role
        {
            get => role;
            set => SetProperty(ref role, value);
        }

        public IAsyncCommand LoadRolesCommand { get; set; }
        public IAsyncCommand RoleSelectedCommand { get; set; }
        public IAsyncCommand SetRolesCommand { get; set; }
        public IAsyncCommand PopCommand { get; set; }
        public Command<GameRole> IncCommand { get; set; }
        public Command<GameRole> DecCommand { get; set; }

        public SetRolesViewModel()
        {
            Roles = new ObservableRangeCollection<NewGameRole>();
            LoadRolesCommand = new AsyncCommand(LoadRoles);
            RoleSelectedCommand=new AsyncCommand(RoleSelected);
            SetRolesCommand = new AsyncCommand(SetRoles);
            PopCommand = new AsyncCommand(Pop);
            IncCommand = new Command<GameRole>(Increment);
            DecCommand = new Command<GameRole>(Decrement);
        }

        private async Task RoleSelected()
        {
            if(Role==null)return;
            if (TotalCount == SelectedCount && !Role.Selected)
            {
                DependencyService.Get<IAlert>().ShortAlert("No more choice",MessageType.Error);
                Role = null;
                return;
            }
            Role.Selected = !Role.Selected;
            if (Role.Selected)
                SelectedCount += Role.Count;
            else
                SelectedCount -= Role.Count;
            Role = null;
        }

        private async Task LoadRoles()
        {
            IsBusy = true;
            var roles = await WebApiService.GetAllRoles();
            if (roles.IsSuccess)
            {
                Roles.Clear();
                Roles.AddRange(roles.Data.Select(s => new NewGameRole()
                {
                    Count = 1,
                    Role = s,
                    AllowInc = s == GameRole.Citizen || s == GameRole.Mafia
                }));
               

                //Device.BeginInvokeOnMainThread(() =>
                //{
                //    CurrentState = !Rooms.Any() ? LayoutState.Empty : LayoutState.None;
                //});
            }
            else
            {
                // CurrentState = LayoutState.Error;
                DependencyService.Get<IAlert>().ShortAlert(roles.Errors.ToString(), MessageType.Error);
            }
            IsBusy = false;
        }

        private async Task Pop()
        {
            await NavigationService.RemovePopupAsync();
        }

        private async Task SetRoles()
        {
            if (TotalCount != SelectedCount)
            {
                DependencyService.Get<IAlert>().ShortAlert("Role selected count must be equal to "+TotalCount, MessageType.Error);
                return;
            }
            SystemConstant.SelectedRoles = Roles.Where(r => r.Selected).ToList();
            MessagingCenter.Send(this, "SetRoles");
            await NavigationService.RemovePopupAsync();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is short count)
            {
                TotalCount = count;
                await LoadRolesCommand.ExecuteAsync();
                if (SystemConstant.SelectedRoles != null && SystemConstant.SelectedRoles.Any())
                {
                    foreach (var selectedRole in SystemConstant.SelectedRoles)
                    {
                        var role = Roles.FirstOrDefault(r => r.Role == selectedRole.Role);
                        role.Selected = true;
                        role.Count = selectedRole.Count;
                    }

                    SelectedCount = SystemConstant.SelectedRoles.Sum(s => s.Count);
                }
            }
            // return base.InitializeAsync(navigationData);
        }

        internal void Increment(GameRole role)
        {
            if (TotalCount == SelectedCount)
                return;
            var selected = Roles.FirstOrDefault(r => r.Role == role);
            selected.Count++;
            if (selected.Selected)
                SelectedCount++;
        }

        internal void Decrement(GameRole role)
        {
            var selected = Roles.FirstOrDefault(r => r.Role == role);
            if (selected.Count == 1) return;
            selected.Count--;
            if (selected.Selected)
                SelectedCount--;
        }

    }
}