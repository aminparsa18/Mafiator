using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Models;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels
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

        public IAsyncRelayCommand LoadRolesCommand { get; set; }
        public IAsyncRelayCommand RoleSelectedCommand { get; set; }
        public IAsyncRelayCommand SetRolesCommand { get; set; }
        public IAsyncRelayCommand PopCommand { get; set; }
        public Command<GameRole> IncCommand { get; set; }
        public Command<GameRole> DecCommand { get; set; }

        private readonly IGamesApiService _gamesApiService;

        public SetRolesViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, 
            IGamesApiService gamesApiService) : base(navigationService, localizer, toastService)
        {
            _gamesApiService = gamesApiService;
            Roles = new ObservableRangeCollection<NewGameRole>();
            LoadRolesCommand = new AsyncRelayCommand(LoadRoles);
            RoleSelectedCommand = new AsyncRelayCommand(RoleSelected);
            SetRolesCommand = new AsyncRelayCommand(SetRoles);
            PopCommand = new AsyncRelayCommand(Pop);
            IncCommand = new Command<GameRole>(Increment);
            DecCommand = new Command<GameRole>(Decrement);
        }

        private async Task RoleSelected()
        {
            if (Role == null) return;
            if (TotalCount == SelectedCount && !Role.Selected)
            {
                _toastService.ShortAlert("No more choice", MessageType.Error);
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
            var roles = new List<GameRole>()
            {
                GameRole.Mafia,
                GameRole.Citizen,
                GameRole.GodFather,
                GameRole.Terrorist,
                GameRole.Detective,
                GameRole.Doctor,
                GameRole.Sniper,
                GameRole.Gun,
                GameRole.Healer,
                GameRole.Immortal,
                GameRole.Natasha,
                GameRole.Priest,
                GameRole.Judge
            };
            Roles.Clear();
            Roles.AddRange(roles.Select(s => new NewGameRole()
            {
                Count = 1,
                Role = s,
                AllowInc = s == GameRole.Citizen || s == GameRole.Mafia
            }));
            IsBusy = false;
        }

        private async Task Pop()
        {
            await _navigationService.RemovePopupAsync();
        }

        private async Task SetRoles()
        {
            if (TotalCount != SelectedCount)
            {
                _toastService.ShortAlert("Role selected count must be equal to " + TotalCount, MessageType.Error);
                return;
            }
            SystemConstant.SelectedRoles = Roles.Where(r => r.Selected).ToList();
            MessagingCenter.Send(this, "SetRoles");
            await _navigationService.RemovePopupAsync();
        }

        public override async Task InitializeAsync(object navigationData)
        {
            if (navigationData is short count)
            {
                TotalCount = count;
                await LoadRolesCommand.ExecuteAsync(null);
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