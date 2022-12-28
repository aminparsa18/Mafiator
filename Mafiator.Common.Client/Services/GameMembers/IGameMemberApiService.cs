using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.GameMembers;

/// <summary>
/// API service provides methods to retrieve/handle game members.
/// </summary>
public interface IGameMemberApiService
{
    /// <summary>
    /// Retrieves all members of a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of game members api result.</returns>
    Task<ApiResult<IEnumerable<GameMemberResult>>> GetMembersOfGame(string gameId);

    /// <summary>
    /// Retrieves waiting players for a game to be started.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of waiting players api result.</returns>
    Task<ApiResult<IEnumerable<WaitingPlayerResult>>> GetWaitingPlayersByGame(string gameId);

    /// <summary>
    /// Retrieves partners of mafia (used only by mafias).
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of mafia players api result.</returns>
    Task<ApiResult<IEnumerable<PlayerRoleResult>>> GetMafiaPartners(string gameId);

    /// <summary>
    /// Retrieves role of player in game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Player role api result.</returns>
    Task<ApiResult<PlayerRoleResult>> GetPlayerRole(string gameId);
}