using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Mafiator.Common.Client.Services.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Models;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class SetRolesViewModel : ViewModelBase
{
    private readonly IGamesApiService _gamesApiService;

    [ObservableProperty]
    private int _selectedCount;

    [ObservableProperty]
    private short _totalCount;

    [ObservableProperty]
    private NewGameRole _role;

    public ObservableRangeCollection<NewGameRole> Roles { get; set; }

    public SetRolesViewModel(INavigationService navigationService, IToastService toastService, 
        IGamesApiService gamesApiService) : base(navigationService, toastService)
    {
        _gamesApiService = gamesApiService;
        Roles = new ObservableRangeCollection<NewGameRole>();
    }

    [RelayCommand]
    private void RoleSelected()
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

    [RelayCommand]
    private void LoadRoles()
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

    [RelayCommand]
    private async Task Pop() => await _navigationService.RemovePopupAsync();

    [RelayCommand]
    private async Task SetRoles()
    {
        if (TotalCount != SelectedCount)
        {
            _toastService.ShortAlert("Role selected count must be equal to " + TotalCount, MessageType.Error);
            return;
        }
        SystemConstant.SelectedRoles = Roles.Where(r => r.Selected).ToList();
        WeakReferenceMessenger.Default.Send(this, "SetRoles");
        await _navigationService.RemovePopupAsync();
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is short count)
        {
            TotalCount = count;
            LoadRolesCommand.Execute(null);
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
        return base.InitializeAsync(navigationData);
    }

    [RelayCommand]
    public void Increment(GameRole role)
    {
        if (TotalCount == SelectedCount)
            return;
        var selected = Roles.FirstOrDefault(r => r.Role == role);
        selected.Count++;
        if (selected.Selected)
            SelectedCount++;
    }

    [RelayCommand]
    public void Decrement(GameRole role)
    {
        var selected = Roles.FirstOrDefault(r => r.Role == role);
        if (selected.Count == 1) return;
        selected.Count--;
        if (selected.Selected)
            SelectedCount--;
    }
}