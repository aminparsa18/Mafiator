using Mafiator.Common.Data.Dtos.Api;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Games;

public interface IGameLeaveService
{
    Task<ApiResult> Leave(string userId, string gameId);
}