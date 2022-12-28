using CommunityToolkit.Mvvm.Input;
using Mafiator.Common.Client.Services.ChatMessages;
using Mafiator.Common.Data.Dtos.ChatMessages;
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Game.Hubs;
using Mafiator.Game.Resources.Texts;
using Mafiator.Game.Services;
using Mafiator.Game.ViewModels.Base;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Localization;

namespace Mafiator.Game.ViewModels;

public class ChatViewModel : ViewModelBase
{
    private bool hubConnected;
    public bool HubConnected
    {
        get => hubConnected;
        set => SetProperty(ref hubConnected, value);
    }

    //message sent on player turn
    private string message;

    public string Message
    {
        get => message;
        set => SetProperty(ref message, value);
    }

    private TimeSpan recordingTimer;
    public TimeSpan RecordingTimer
    {
        get => recordingTimer;
        set => SetProperty(ref recordingTimer, value);
    }

    private string roomId;
    public ObservableRangeCollection<ChatMessageResult> Messages { get; set; }
    public ObservableRangeCollection<RoomMemberResult> Members { get; set; }
    public IAsyncRelayCommand SendMessageCommand { get; set; }

    private readonly IChatMessagesApiService _chatMessagesApiService;

    public ChatViewModel(INavigationService navigationService, IStringLocalizer<AppResources> localizer, IToastService toastService, IChatMessagesApiService chatMessagesApiService) : base(navigationService, localizer, toastService)
    {
        _chatMessagesApiService = chatMessagesApiService;
        Messages = new ObservableRangeCollection<ChatMessageResult>();
        SendMessageCommand = new AsyncRelayCommand(SendMessage);
    }

    private async Task SendMessage()
    {
        if (string.IsNullOrEmpty(Message))
            return;
        Messages.Add(new ChatMessageResult()
        {
            Content = Message,
            Type = GameMessageType.Text,
            //Image = Members.FirstOrDefault(m => m.UserId == )?.Image,
            //DisplayName = Members.FirstOrDefault(m => m.Id == player.MemberId)?.DisplayName,
        });
        await ChatHub.Instance.InvokeAsync("SendMessage", roomId, Message, "text", "");
        Message = "";
    }

    public override async Task InitializeAsync(object navigationData)
    {
        if (navigationData is string roomId)
        {
            this.roomId = roomId;
            await LoadMessages();
        }
    }

    private async Task LoadData()
    {
        await Task.WhenAll(StartHub(), LoadMessages());
    }

    private async Task StartHub()
    {
        ChatHub.Instance.Reconnecting += HubReconnecting;
        ChatHub.Instance.Reconnected += HubReconnected;
        ChatHub.Instance.Closed += HubClosed;
        if (ChatHub.Instance.State == HubConnectionState.Disconnected)
            await ChatHub.Instance.StartAsync();
        HubConnected = true;
        await ChatHub.Instance.InvokeAsync("Subscribe", roomId);
        GameHub.Instance.On<string, string, string>("SendMessage", MessageReceived);
    }

    private async Task LoadMessages()
    {
        var res = await _chatMessagesApiService.GetChatByRoom(roomId);
        if (res.IsSuccess)
            Messages.AddRange(res.Data);
        else
        {
            _toastService.ShortAlert(res.Errors.FirstOrDefault(), MessageType.Error);
        }
    }

    private void MessageReceived(string msg, string type, string sender)
    {
        var message = new ChatMessageResult()
        {
            Content = msg,
            Image = Members.FirstOrDefault(m => m.UserId == Guid.Parse(sender))?.Image,
            DisplayName = Members.FirstOrDefault(m => m.UserId == Guid.Parse(sender))?.Name,
        };
        message.Type = type switch
        {
            "text" => GameMessageType.Text,
            "voice" => GameMessageType.Voice,
            _ => message.Type
        };
        //if (message.Type == GameMessageType.Voice)
        //    message.CurrentState = LayoutState.Loading;
        Messages.Add(message);
    }

    private async Task HubClosed(Exception arg)
    {
        HubConnected = false;
        _toastService.ShortAlert("Chat Hub Closed", MessageType.Info);
        await Task.Delay(100);
    }

    private async Task HubReconnected(string arg)
    {
        HubConnected = true;
        await ChatHub.Instance.InvokeAsync("Subscribe", roomId);
    }

    private Task HubReconnecting(Exception arg)
    {
        HubConnected = false;
        return Task.CompletedTask;
    }
}