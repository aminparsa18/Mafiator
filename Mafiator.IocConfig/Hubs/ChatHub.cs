using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;

namespace Mafiator.IocConfig.Hubs
{
    public class ChatHub:Hub
    {
        public async Task JoinGroup(string gpName)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gpName);
            await Clients.Group(gpName).SendAsync("JoinNotif", $"{Context.ConnectionId} has joined the group.");
        }
        public async Task RemoveFromGroup(string groupName)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, groupName);
            await Clients.Group(groupName).SendAsync("LeftNotif", $"{Context.ConnectionId} has left the group.");
        }
        public async Task SendMessage(string groupName,string msg,string type)
        {
            await Clients.OthersInGroup(groupName).SendAsync("MessageReceived", msg,type);
        }
    }
}
