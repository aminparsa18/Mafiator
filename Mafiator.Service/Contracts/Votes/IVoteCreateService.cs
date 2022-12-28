using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Votes;

public interface IVoteCreateService
{
    Task<ApiResult> Create(VoteCreateRequest request);
}