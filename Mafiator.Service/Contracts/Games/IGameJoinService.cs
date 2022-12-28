using Mafiator.Common.Data.Dtos.Api;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Games;

public interface IGameJoinService
{
    Task<ApiResult> Join(string userId, string gameId);
}