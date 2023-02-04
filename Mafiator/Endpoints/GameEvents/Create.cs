using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts.GameEvents;

namespace Mafiator.Api.Endpoints.GameEvents;

public class Create : Endpoint<GameEventRequest,ApiResult>
{
    private readonly IGameEventCreateService _gameEventCreateService;

    public Create(IGameEventCreateService gameEventCreateService)
    {
        _gameEventCreateService = gameEventCreateService;
    }

    public override void Configure()
    {
        Post("api/v1/gameevents");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.GameEvents));
    }

    public override async Task HandleAsync(GameEventRequest request, CancellationToken ct) => 
        await SendMemoryPackAsync(await _gameEventCreateService.Create(request), cancellation: ct);
}