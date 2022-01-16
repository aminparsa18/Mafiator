using Mafiator.Entities;
using Mafiator.Entities.Enums;
using Mafiator.Repository;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Threading.Tasks;

namespace Mafiator.IocConfig.Hubs
{
    public class ChatHub:Hub
    {
        private readonly IUnitOfWork _unitOfWork;

        public ChatHub(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task SendMessage(string groupName, string msg, string type, string sender)
        {
            await Clients.OthersInGroup(groupName).SendAsync("MessageReceived", msg, type, sender);
            await _unitOfWork.ChatMessage.AddFast(new ChatMessage()
            {
                Content = msg,
                RoomId = Guid.Parse(groupName),
                MessageType = type switch
                {
                    "text" => GameMessageType.Text,
                    "voice" => GameMessageType.Voice,
                    _ => GameMessageType.Video
                },
                UserId = Guid.Parse(sender)
            });
        }
    }
}
