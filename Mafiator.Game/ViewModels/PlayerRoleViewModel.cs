using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Cache;
using Mafiator.Common.Client.Services.GameMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Models.Game;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;

namespace Mafiator.Game.ViewModels;

public partial class PlayerRoleViewModel : ViewModelBase
{
    private int timer;
    private string gameId;
    private bool first;
    private bool fromDetail;

    private readonly IGameMemberApiService _gameMemberApiService;

    [ObservableProperty]
    private GameRole? role;

    [ObservableProperty]
    private double progressTimer;

    [ObservableProperty]
    private bool canClose;

    [ObservableProperty]
    private bool isMafia;

    public IAsyncRelayCommand PopCommand { get; set; }
    public ObservableRangeCollection<PartnerDto> Partners { get; set; }

    public PlayerRoleViewModel(INavigationService navigationService, IToastService toastService, 
        IGameMemberApiService gameMemberApiService) : base(navigationService, toastService)
    {
        _gameMemberApiService = gameMemberApiService;
        Partners = new ObservableRangeCollection<PartnerDto>();
        PopCommand = new AsyncRelayCommand(Pop);
    }

    private async Task Pop()
    {
        await _navigationService.RemovePopupAsync();
    }

    private async Task LoadRole()
    {
        if (first)
            Dispatcher.GetForCurrentThread().StartTimer(TimeSpan.FromMilliseconds(100), () =>
            {
                timer += 100;
                ProgressTimer = 100 * (double)timer / 40000;
                if (timer != 40000)
                    return true;
                CanClose = true;
                return false;
            });
        else
            CanClose = true;

        if (fromDetail) return;
        IsMafia = Role is GameRole.Mafia or GameRole.GodFather;
        if (IsMafia)
        {
            var partners = await _gameMemberApiService.GetMafiaPartners(gameId);
            if (partners.IsSuccess)
            {
                var members = Barrel.Current.Get<IEnumerable<PlayerDetails>>("Members");
                Partners.AddRange(partners.Data.Select(s => new PartnerDto()
                {
                    Role = s.Role,
                    Name = members.FirstOrDefault(m => m.Id == s.MemberId)?.DisplayName
                }));
            }
        }

    }

    public override async Task InitializeAsync(object navigationData)
    {
        if (navigationData is Tuple<GameRole?, string, bool> data)
        {
            Role = data.Item1;
            gameId = data.Item2;
            first = data.Item3;
            await LoadRole();
        }
        else if (navigationData is Tuple<GameRole?, bool> data2)
        {
            Role = data2.Item1;
            fromDetail = true;
            await LoadRole();
        }
    }
}