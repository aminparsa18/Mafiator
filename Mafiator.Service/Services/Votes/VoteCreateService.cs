using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Entities.Models;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Votes;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Votes;

public class VoteCreateService : IVoteCreateService
{
    private readonly IValidator<VoteCreateRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public VoteCreateService(IUnitOfWork unitOfWork, IValidator<VoteCreateRequest> validator)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult> Create(VoteCreateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };
        await _unitOfWork.Vote.AddRangeFast(request.Targets.Select(s => new Vote()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            GameId = request.GameId,
            TargetId = s,
            VoterId = request.VoterId
        }));
        return new ApiResult
        {
            IsSuccess = true
        };
    }
}