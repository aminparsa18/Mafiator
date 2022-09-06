using Mafiator.Common.Data.Dtos.Gems;
using System.Collections.Generic;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle gem data.
/// </summary>
public interface IGemRepository : IBaseRepository<Gem>
{
    /// <summary>
    /// Retrieves all gems.
    /// </summary>
    /// <returns>List of gems.</returns>
    Task<IEnumerable<GemResult>> GetAllDto();
}