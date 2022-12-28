using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Votes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Votes;

public class VoteService : IVoteService
{
    private readonly IUnitOfWork _unitOfWork;

    public VoteService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IEnumerable<VoteDetailsResult>>> GetByGame(string gameId)
    {
        return new ApiResult<IEnumerable<VoteDetailsResult>>()
        {
            Data = await _unitOfWork.Vote.GetVoteStatus(gameId),
            IsSuccess = true
        };
    }
}