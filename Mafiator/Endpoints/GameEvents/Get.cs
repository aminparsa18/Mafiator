using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Repository;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameEvents;

public class Get : EndpointWithoutRequest<ApiResult<IEnumerable<GameEventResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override void Configure()
    {
        Get("api/v1/gameevents/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.GameEvents));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(new ApiResult<IEnumerable<GameEventResult>>()
        {
            IsSuccess = true,
            Data = await _unitOfWork.GameEvent.GetByGame(gameId)
        }, cancellation: ct);
    }
}