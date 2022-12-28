using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.GameMembers;

public interface IGameMemberService
{
    Task<ApiResult<IEnumerable<GameMemberResult>>> GetByGame(string gameId);

    Task<ApiResult<IEnumerable<PlayerRoleResult>>> GetMafiaPartners(string userId, string gameId);

    Task<ApiResult<PlayerRoleResult>> GetPlayerRole(string userId, string gameId);

    Task<ApiResult<IEnumerable<WaitingPlayerResult>>> GetWaitingPlayersByGame(string gameId);
}