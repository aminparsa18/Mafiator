using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Gems;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Gems
{
    /// <inheritdoc/>
    public class GemsApiService : IGemsApiService
    {
        /// <inheritdoc/>
        public Task<ApiResult<IEnumerable<GemResult>>> GetAllGems()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<GemResult>>>(
                new Uri($"{UrlConstants.BaseUrl}gems"));
        }
    }
}