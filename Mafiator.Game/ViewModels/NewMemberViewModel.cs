using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Client.Services.RoomMembers;
using Mafiator.Common.Client.Services.Users;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Game.Models.PipeEvents;
using Mafiator.Game.Services;
using Mafiator.Game.Validations;
using Mafiator.Game.ViewModels.Base;
using MessagePack;
using MessagePipe;
using System.Net;

namespace Mafiator.Game.ViewModels;

public partial class NewMemberViewModel : ViewModelBase
{
    private Guid roomId;

    private readonly IPublisher<UpdateRoomEvent> _publisher;
    private readonly IRoomMembersApiService _roomMembersApiService;
    private readonly IUsersApiService _usersApiService;

    [ObservableProperty]
    private ValidatableObject<string> name;

    [ObservableProperty]
    private bool isNameValid;

    public IAsyncRelayCommand AddMemberCommand { get; set; }
    public IAsyncRelayCommand PopCommand { get; set; }
    public IAsyncRelayCommand ScanQrCommand { get; set; }
    public IAsyncRelayCommand SearchMemberCommand { get; set; }
    public ObservableRangeCollection<ValidateUserResult> Members { get; set; }

    public NewMemberViewModel(INavigationService navigationService, IToastService toastService, IPublisher<UpdateRoomEvent> publisher,
        IRoomMembersApiService roomMembersApiService, IUsersApiService usersApiService) : base(navigationService, toastService)
    {
        _publisher = publisher;
        _roomMembersApiService = roomMembersApiService;
        _usersApiService = usersApiService;
        Members = new ObservableRangeCollection<ValidateUserResult>();
        Name = new ValidatableObject<string>();
        AddValidations();
        AddMemberCommand = new AsyncRelayCommand(AddMember);
        PopCommand = new AsyncRelayCommand(Pop);
        ScanQrCommand = new AsyncRelayCommand(ScanQr);
        SearchMemberCommand = new AsyncRelayCommand(SearchMember);
        if (SystemConstant.Members != null && SystemConstant.Members.Any())
            Members.AddRange(SystemConstant.Members);
    }

    private async Task SearchMember()
    {
        //  if (Guid.TryParse(Name.Value, out var id))
        //{
        try
        {
            await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Finding User...");
            var member = await _usersApiService.ValidateUser(Name.Value);
            if (Members.Any(m => m.Id == member.Data.Id))
            {
                await _navigationService.RemovePopupAsync();
                _toastService.ShortAlert("User already exist in your collection", MessageType.Error);
                return;
            }

            Members.Add(member.Data);
            Name.Value = "";
            await _navigationService.RemovePopupAsync();
        }
        catch (MessagePackSerializationException)
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert("No such user", MessageType.Error);
        }

        //}
        //else
        //{
        //    DependencyService.Get<IAlert>().ShortAlert("Invalid User Code", MessageType.Error);
        //}
    }


    private async Task Pop()
    {
        await _navigationService.RemovePopupAsync();
    }

    private async Task AddMember()
    {
        if (roomId == Guid.Empty)
        {
            SystemConstant.Members = Members.ToList();
            await Pop();
            return;
        }

        if (!Members.Any())
        {
            _toastService.ShortAlert("No user selected", MessageType.Error);
            return;
        }
        await _navigationService.NavigateToPopupAsync<WaitingViewModel>("Adding members...");
        var response = await _roomMembersApiService.AddMember(new NewMembersRequest()
        {
            RoomId = roomId,
            Users = Members.Select(s => s.Id).ToList()
        });

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsMemoryPackAsync<ApiResult<string>>();
            if (result.IsSuccess)
            {
                await _navigationService.RemovePopupAsync();
                await _navigationService.RemovePopupAsync();
                _toastService.ShortAlert("Member(s) joined room", MessageType.Success);
                _publisher.Publish(new UpdateRoomEvent());
            }
            else
            {
                await _navigationService.RemovePopupAsync();
                _toastService.ShortAlert(string.Join(',', result.Errors), MessageType.Error);
            }
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            await _navigationService.RemovePopupAsync();
            _toastService.ShortAlert("No such user found", MessageType.Error);
        }
        else
        {
            await _navigationService.RemovePopupAsync();
            var content = await response.Content.ReadAsStringAsync();
            _toastService.ShortAlert(content, MessageType.Error);
        }
    }

    public override Task InitializeAsync(object navigationData)
    {
        if (navigationData is Guid id)
            roomId = id;
        return base.InitializeAsync(navigationData);
    }

    private async Task ScanQr()
    {
        await _navigationService.NavigateToAsync(nameof(BarcodeScannerViewModel));
    }

    private void AddValidations()
    {
        Name.Validations.Add(new IsNotNullOrEmptyRule<string>() { ValidationMessage = "نام کاربری را وارد کنید" });
    }

    private bool Validate()
    {
        IsNameValid = Name.Validate();
        return IsNameValid;
    }

    public void RemoveMember(Guid userId)
    {
        Members.Remove(Members.FirstOrDefault(m => m.Id == userId));
    }
}