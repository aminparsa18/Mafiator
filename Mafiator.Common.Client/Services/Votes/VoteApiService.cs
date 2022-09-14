using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Votes
{
    /// <inheritdoc/>
    public class VoteApiService : IVotesApiService
    {
        /// <inheritdoc/>
        public Task<ApiResult<IEnumerable<VoteDetailsResult>>> GetVotesStatus(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<VoteDetailsResult>>>(
               new Uri($"{UrlConstants.BaseUrl}votes/{gameId}"));
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> SendVotes(VoteCreateRequest request)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri($"{UrlConstants.BaseUrl}votes"), request);
        }
    }
}