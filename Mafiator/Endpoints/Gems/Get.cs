using Mafiator.Common.Data.Dtos.Gems;
using Mafiator.Repository;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Gems;

public class Get : EndpointWithoutRequest<ApiResult<IEnumerable<GemResult>>>
{
    private readonly IUnitOfWork _unitOfWork;
    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("api/v1/gems");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d =>d.Produces(200).WithTags(EndpointsTags.Gems));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendMemoryPackAsync(new ApiResult<IEnumerable<GemResult>>()
        {
            IsSuccess = true,
            Data = await _unitOfWork.Gem.GetAllDto()
        }, cancellation: ct);
    }
}