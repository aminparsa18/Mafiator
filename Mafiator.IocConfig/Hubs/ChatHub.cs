using System;
using System.Threading.Tasks;
using Mafiator.Entities;
using Mafiator.Entities.Enums;
using Mafiator.Repository;
using Microsoft.AspNetCore.SignalR;

namespace Mafiator.IocConfig.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IUnitOfWork unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task SendMessage(string groupName, string msg, string type, string sender)
        {
            await Clients.OthersInGroup(groupName).SendAsync("MessageReceived", msg, type, sender);
            await unitOfWork.ChatMessage.AddFast(new ChatMessage()
            {
                Content = msg,
                RoomId = Ulid.Parse(groupName),
                MessageType = type switch
                {
                    "text" => GameMessageType.Text,
                    "voice" => GameMessageType.Voice,
                    _ => GameMessageType.Video
                },
                UserId = Ulid.Parse(sender)
            });
        }
    }
}
