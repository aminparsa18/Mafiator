using Hangfire;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace Mafiator.Service.Hubs;

[Authorize]
public class GameHub : Hub
{
    private readonly IMemoryCache cache;

    public GameHub(IMemoryCache cache)
    {
        this.cache = cache;
    }

    public async Task Subscribe(string gameId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, gameId);
    }

    public async Task Unsubscribe(string gameName)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, gameName);
    }

    public async Task SendMessage(string groupName, string msg, string type, string sender)
    {
        await Clients.OthersInGroup(groupName).SendAsync("MessageReceived", msg, type, sender);
    }

    public async Task MafiaChoice(string groupName, string memberId)
    {
        await Clients.OthersInGroup(groupName).SendAsync("MafiaChose", memberId);
    }

    public void Next(string groupName)
    {
        var jobId = cache.GetCache<string>("JobId" + groupName);
        BackgroundJob.Requeue(jobId);
    }
}