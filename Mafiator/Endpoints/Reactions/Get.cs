using FastEndpoints;
using Mafiator.Common.Data.Dtos.Reactions;
using Mafiator.Repository;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Reactions;

public class Get : EndpointWithoutRequest<ApiResult<IEnumerable<ReactionResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("api/v1/reactions");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Reactions));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendMemoryPackAsync(new ApiResult<IEnumerable<ReactionResult>>()
        {
            IsSuccess = true,
            Data = await _unitOfWork.Reaction.GetAllDtos()
        }, cancellation: ct);
    }
}