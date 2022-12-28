using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Avatars;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Avatars;

/// <summary>
/// API service provides methods to retrieve/handle avatars.
/// </summary>
public interface IAvatarsApiService
{
    /// <summary>
    /// Retrieves all avatars data.
    /// </summary>
    /// <returns></returns>
    Task<ApiResult<IEnumerable<AvatarResult>>> GetAllAvatars();
}