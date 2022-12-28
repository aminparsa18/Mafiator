using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Votes;

/// <summary>
/// API service provides methods to retrieve/handle votes.
/// </summary>
public interface IVotesApiService
{
    /// <summary>
    /// Vote someone in a game.
    /// </summary>
    /// <param name="request">Vote create request.</param>
    /// <returns>HTTP response message.</returns>
    Task<HttpResponseMessage> SendVotes(VoteCreateRequest request);

    /// <summary>
    /// Retrieves all vote statuses in a game.
    /// </summary>
    /// <param name="gameId">Game key identifier.</param>
    /// <returns>List of vote details api result.</returns>
    Task<ApiResult<IEnumerable<VoteDetailsResult>>> GetVotesStatus(string gameId);
}