using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Data.Dtos.Vote;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle vote data.
/// </summary>
public interface IVoteRepository : IBaseRepository<Vote>
{
    /// <summary>
    /// Update flags to validates votes in game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns></returns>
    Task<int> Validate(string gameId);

    /// <summary>
    /// Retrieves all not validated target of game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of targets.</returns>
    Task<IEnumerable<VoteValidateDto>> GetNonValidatedTargets(string gameId);

    /// <summary>
    /// Retrieves all votes statuses of game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of votes.</returns>
    Task<IEnumerable<VoteDetailsResult>> GetVoteStatus(string gameId);
}