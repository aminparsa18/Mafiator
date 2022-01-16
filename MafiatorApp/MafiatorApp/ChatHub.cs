using MafiatorApp.Cache;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace MafiatorApp
{
    public class ChatHub
    {
        private static HubConnection _instance;

        private ChatHub()
        {
        }

        public static HubConnection Instance
        {
            get { return _instance ??= CreateInstance(); }
        }

        private static HubConnection CreateInstance()
        {
            return new HubConnectionBuilder()
                .WithUrl("https://mafiatorapi.azurewebsites.net/chathub",
                    options =>
                    {
                        options.AccessTokenProvider = () => Task.FromResult(Barrel.Current.Get<string>("Token"));
                    }).ConfigureLogging(logging => { logging.AddConsole(); }).WithAutomaticReconnect()
                .AddMessagePackProtocol().Build();
        }

        public static async Task Dispose()
        {
            await Instance.DisposeAsync();
        }
    }
}