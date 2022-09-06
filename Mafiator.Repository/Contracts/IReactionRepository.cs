using Mafiator.Common.Data.Dtos.Reactions;
using System.Collections.Generic;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle reaction data.
/// </summary>
public interface IReactionRepository : IBaseRepository<Reaction>
{
    /// <summary>
    /// Retrieves all reactions.
    /// </summary>
    /// <returns>List of reactions.</returns>
    Task<IEnumerable<ReactionResult>> GetAllDtos();
}