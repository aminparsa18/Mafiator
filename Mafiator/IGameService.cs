using System.Threading.Tasks;

namespace Mafiator.Api
{
    public interface IGameService
    {
        Task SetTurn(string gameId,int index);
    }
}
