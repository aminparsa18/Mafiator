using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Games;

public interface IGameLogicService
{
    Task SetTurn(string gameId, int index);
}