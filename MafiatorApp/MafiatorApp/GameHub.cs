using System.Threading.Tasks;
using MafiatorApp.Cache;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MafiatorApp
{
    public class GameHub
    {
        private static HubConnection _instance;

        private GameHub()
        {
        }

        public static HubConnection Instance
        {
            get
            {
                    return _instance ??= CreateInstance();
            }
        }
        private static HubConnection CreateInstance()
        {
           return new HubConnectionBuilder().WithUrl("https://api.mafiator.com/gamehub", options =>
                {
                    options.AccessTokenProvider = () => Task.FromResult(Barrel.Current.Get<string>("Token"));
                }).ConfigureLogging(logging =>
               {
                   logging.AddConsole();
               }).WithAutomaticReconnect()
                .AddMessagePackProtocol().Build();
        }

        public static async Task Dispose()
        {
            await Instance.DisposeAsync();
        }
    }
}
