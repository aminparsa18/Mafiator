using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Votes;

public interface IVoteService
{
    Task<ApiResult<IEnumerable<VoteDetailsResult>>> GetByGame(string gameId);
}