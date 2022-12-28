using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Gems;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Gems;

/// <summary>
/// API service provides methods to retrieve/handle gems.
/// </summary>
public interface IGemsApiService
{
    /// <summary>
    /// Retrieves all gems.
    /// </summary>
    /// <returns>List of gems api result.</returns>
    Task<ApiResult<IEnumerable<GemResult>>> GetAllGems();
}