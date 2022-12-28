using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Games;

/// <summary>
/// API service provides methods to retrieve/handle games.
/// </summary>
public interface IGamesApiService
{
    /// <summary>
    /// Creates a new game.
    /// </summary>
    /// <param name="game">Game create request.</param>
    /// <returns>Http respose message.</returns>
    Task<HttpResponseMessage> AddGame(GameCreateRequest game);

    /// <summary>
    /// Retrieves all games played in a room.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>List of games played api result.</returns>
    Task<ApiResult<IEnumerable<RoomGameResult>>> GetGamesByRoom(string roomId);

    /// <summary>
    /// Retrieves all available games to play.
    /// </summary>
    /// <returns>List of available games api result.</returns>
    Task<ApiResult<IEnumerable<AvailableGameResult>>> GetAvailableGames();

    /// <summary>
    /// Retrieves appointed game in a room if exists.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Appointed game api result.</returns>
    Task<ApiResult<AppointedGameResult>> GetAppointedGame(string roomId);

    /// <summary>
    /// Retrieved appointed game details.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Appointed game details api result.</returns>
    Task<ApiResult<AppointedGameResult>> GetAppointedGameDetails(string gameId);

    /// <summary>
    /// Retrieved member identifier if is joined in the game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Game member key identifier api result.</returns>
    Task<ApiResult<string>> IsGameJoined(string gameId);

    /// <summary>
    /// Joins a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Http response message.</returns>
    Task<HttpResponseMessage> JoinGame(string gameId);

    /// <summary>
    /// Leaves a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Http response message.</returns>
    Task<HttpResponseMessage> LeaveGame(string gameId);
}