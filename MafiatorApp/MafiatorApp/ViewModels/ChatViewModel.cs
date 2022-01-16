using MafiatorApp.Dtos;
using MafiatorApp.Dtos.Room;
using MafiatorApp.Enums;
using MafiatorApp.Services;
using MafiatorApp.ViewModels.Base;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace MafiatorApp.ViewModels
{
    public class ChatViewModel:ViewModelBase
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
        public ObservableRangeCollection<ChatMessageDto> Messages { get; set; }
        public ObservableRangeCollection<RoomMemberDto> Members { get; set; }
        public IAsyncCommand SendMessageCommand { get; set; }

        public ChatViewModel()
        {
            Messages=new ObservableRangeCollection<ChatMessageDto>();
            SendMessageCommand = new AsyncCommand(SendMessage);

        }

        private async Task SendMessage()
        {
            if (string.IsNullOrEmpty(Message))
                return;
            Messages.Add(new ChatMessageDto()
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
            var res=await WebApiService.GetChatByRoom(roomId);
            if (res.IsSuccess)
            {
                Messages.AddRange(res.Data);
            }
            else
            {
                DependencyService.Get<IAlert>().ShortAlert(res.Errors.FirstOrDefault(),MessageType.Error);
            }
        }

        private void MessageReceived(string msg, string type, string sender)
        {
            var message = new ChatMessageDto()
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
            DependencyService.Get<IAlert>().ShortAlert("Chat Hub Closed", MessageType.Info);
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
}
