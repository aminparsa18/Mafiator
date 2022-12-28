using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Avatars;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Avatars;

/// <inheritdoc/>
public class AvatarsApiService : IAvatarsApiService
{
    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<AvatarResult>>> GetAllAvatars()
    {
        return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<AvatarResult>>>(
           new Uri($"{UrlConstants.BaseUrl}avatars"));
    }
}