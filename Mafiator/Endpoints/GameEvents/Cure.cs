using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts.GameEvents;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.GameEvents;

public class Cure : Endpoint<GameEventRequest,ApiResult>
{
    private readonly IGameEventCureService _gameEventCureService;

    public Cure(IGameEventCureService gameEventCureService)
    {
        _gameEventCureService = gameEventCureService;
    }

    public override void Configure()
    {
        Post("api/v1/gameevents/cure");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.GameEvents));
    }

    public override async Task HandleAsync(GameEventRequest request, CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        await SendMemoryPackAsync(await _gameEventCureService.Cure(userId, request), cancellation: ct);
    }
}