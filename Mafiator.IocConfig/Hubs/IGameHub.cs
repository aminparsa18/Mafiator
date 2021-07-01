using System.Threading.Tasks;

namespace Mafiator.IocConfig.Hubs
{
    public interface IGameHub
    {
        Task JoinGame(string gameId);
    }
}
