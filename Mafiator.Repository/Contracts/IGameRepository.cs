using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.Room;
using System;
using System.Collections.Generic;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle game data.
/// </summary>
public interface IGameRepository : IBaseRepository<Game>
{
    /// <summary>
    /// Retrieves all games in a room.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>List of games.</returns>
    Task<IEnumerable<RoomGameDto>> GetByRoom(string roomId);

    /// <summary>
    /// Retrieves informations of a waiting game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Waiting game informations.</returns>
    Task<WaitingGameDto> GetWaitingGameInformations(Guid gameId);

    /// <summary>
    /// Retrieves informations of a waiting game.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Waiting game informations.</returns>
    Task<WaitingGameDto> GetWaitingGameByRoom(Guid roomId);

    /// <summary>
    /// Retrieves available games to play.
    /// </summary>
    /// <returns>List of available games.</returns>
    Task<List<GameDto>> GetAvailables();

    /// <summary>
    /// Rertieves game member key identifier if is joined.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Game member key identifier.</returns>
    Task<string> IsJoinedFast(string userId, string gameId);

    /// <summary>
    /// Retrieves game member key identifier if is already playing.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns></returns>
    Task<string> IsAlreadyPlaying(string roomId);

    /// <summary>
    /// Update flag of game status to started.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Number of affected rows.</returns>
    Task<int> StartGame(string gameId);

    /// <summary>
    /// Upodate flag of game for mafia win.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns></returns>
    Task<int> MafiaWin(string gameId);

    /// <summary>
    /// Update flag of game for citizen win.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns></returns>
    Task<int> CitizenWin(string gameId);
}