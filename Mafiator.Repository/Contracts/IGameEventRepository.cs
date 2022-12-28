using Mafiator.Data.Dtos.GameEvent;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle game event data.
/// </summary>
public interface IGameEventRepository : IBaseRepository<GameEvent>
{
    /// <summary>
    /// Retrieves game events by game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of game events.</returns>
    Task<IEnumerable<GameEventResult>> GetByGame(string gameId);

    /// <summary>
    /// Validates all not validated game events by game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>Number of rows affected.</returns>
    Task<int> Validate(string gameId);
}