using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.User;
using System;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle chat game member data.
/// </summary>
public interface IGameMemberRepository : IBaseRepository<GameMember>
{
    /// <summary>
    /// Retrieves user status based on played games.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <returns>List of user game statuses.</returns>
    Task<List<UserGameStatus>> GetUserStatus(Guid userId);

    /// <summary>
    /// Retrieves user status based on played games.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <returns>List of user game statuses.</returns>
    Task<IEnumerable<UserGameStatus>> GetUserStatusFast(string userId);

    /// <summary>
    /// Retrieves game members by game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of game members.</returns>
    Task<List<GameMemberResult>> GetByGame(Guid gameId);

    /// <summary>
    /// Retrieves game members by game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of game members.</returns>
    Task<IEnumerable<GameMemberResult>> GetByGameFast(string gameId);

    /// <summary>
    /// Retrieves waiting players for a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of waiting players.</returns>
    Task<IEnumerable<WaitingPlayerResult>> GetWaitingPlayersByGame(string gameId);

    /// <summary>
    /// Retrieves role of player in a game.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Player role (first in list).</returns>
    Task<IEnumerable<PlayerRoleResult>> GetRoleOfPlayer(string userId, string gameId);

    /// <summary>
    /// Retrieves players in a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of players.</returns>
    Task<IEnumerable<PlayerDetails>> GetPlayerByGame(string gameId);

    /// <summary>
    /// Retrieves players in a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of players.</returns>
    Task<IEnumerable<PlayerDetails>> GetUsersByGame(string gameId);

    /// <summary>
    /// Retrives players count in a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns></returns>
    Task<int> CountPlayerByGame(string gameId);

    /// <summary>
    /// Retrieves player with specific role.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <param name="role">Game role.</param>
    /// <returns>Player role.</returns>
    Task<IEnumerable<PlayerRoleResult>> GetPlayerByRoleFast(string gameId, short role);

    /// <summary>
    /// Retrieves partners of a player as mafia.
    /// </summary>
    /// <param name="memberId">Member key identifier.</param>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of mafia partners.</returns>
    Task<IEnumerable<PlayerRoleResult>> GetMafiaPartners(string memberId, string gameId);

    /// <summary>
    /// Retrieves Player role by user in a game.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Player role</returns>
    Task<IEnumerable<PlayerRoleResult>> GetUser(string userId, string gameId);

    /// <summary>
    /// Join a game.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <param name="memberId">Game member key identifier.</param>
    /// <returns>Number of affected rows.</returns>
    Task<int> Join(string userId, string memberId);

    /// <summary>
    /// Leave a game.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <returns>Number of affected rows.</returns>
    Task<int> Leave(string userId);

    /// <summary>
    /// Kick a member out of game.
    /// </summary>
    /// <param name="memberId">Game member key identifier.</param>
    /// <returns></returns>
    Task<int> KickMember(string memberId);

    /// <summary>
    /// Kill a player in game.
    /// </summary>
    /// <param name="memberId">Game member identifier.</param>
    /// <returns></returns>
    Task<int> KillMember(string memberId);

    /// <summary>
    /// Silence a member in game.
    /// </summary>
    /// <param name="memberId">Game member key identifier.</param>
    /// <returns></returns>
    Task<int> SilenceMember(string memberId);

    /// <summary>
    /// Inquire player status (for detective)
    /// </summary>
    /// <param name="memberId">Game member key identifier.</param>
    /// <returns>Player status.</returns>
    Task<IEnumerable<UserGameStatus>> GetPlayerStatusFast(string memberId);
}