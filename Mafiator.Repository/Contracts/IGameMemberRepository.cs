using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.User;
using Mafiator.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Repository.Contracts
{
    public interface IGameMemberRepository:IRepository<GameMember>
    {
        Task<List<UserGameStatusDto>> GetUserStatus(Guid userId);
        Task<IEnumerable<UserGameStatusDto>> GetUserStatusFast(string userId);
        Task<List<GameMemberDto>> GetByGame(Guid gameId);
        Task<IEnumerable<GameMemberDto>> GetByGameFast(string gameId);
        Task<IEnumerable<WaitingPlayerDto>> GetWaitingPlayersByGame(string gameId);
        Task<IEnumerable<PlayerRoleDto>> GetRoleOfPlayer(string userId, string gameId);
        Task<IEnumerable<PlayerDto>> GetPlayerByGame(string gameId);
        Task<IEnumerable<PlayerDto>> GetUsersByGame(string gameId);
        Task<int> CountPlayerByGame(string gameId);
        Task<IEnumerable<PlayerRoleDto>> GetPlayerByRoleFast(string gameId, short role);
        Task<IEnumerable<PlayerRoleDto>> GetMafiaPartners(string memberId, string gameId);
        Task<IEnumerable<PlayerRoleDto>> GetUser(string userId, string gameId);
        Task<int> Join(string userId, string memberId);
        Task<int> Leave(string userId);
        Task<int> KickMember(string memberId);
        Task<int> KillMember(string memberId);
        Task<int> SilenceMember(string memberId);
        //for detective
        Task<IEnumerable<UserGameStatusDto>> GetPlayerStatusFast(string memberId);
    }
}
