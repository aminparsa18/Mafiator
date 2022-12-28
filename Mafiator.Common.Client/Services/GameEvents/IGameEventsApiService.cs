using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.GameEvents;

/// <summary>
/// API service provides methods to retrieve/handle game events.
/// </summary>
public interface IGameEventsApiService
{
    /// <summary>
    /// Creates a new game event.
    /// </summary>
    /// <param name="gameEvent">Game event request.</param>
    /// <returns>HTTP response message.</returns>
    Task<HttpResponseMessage> FireGameEvent(GameEventRequest gameEvent);

    /// <summary>
    /// Cures someone (used by doctor).
    /// </summary>
    /// <param name="gameEvent">Game event request.</param>
    /// <returns>HTTP response message.</returns>
    Task<HttpResponseMessage> Cure(GameEventRequest gameEvent);

    /// <summary>
    /// Inquiry if someone is mafia or not (used by detective).
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    Task<HttpResponseMessage> Inquiry(GameEventRequest gameEvent);

    /// <summary>
    /// Retrieves game events by game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of game events.</returns>
    Task<ApiResult<IEnumerable<GameEventResult>>> GetEventStatus(string gameId);

    /// <summary>
    /// Retrieved events happened at night.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of game events.</returns>
    Task<ApiResult<IEnumerable<GameEventResult>>> GetNightResult(string gameId);
}