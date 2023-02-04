using Mafiator.Service.Contracts.Games;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Games;

public class Join : EndpointWithoutRequest<ApiResult>
{
    private readonly IGameJoinService _gameJoinService;

    public Join(IGameJoinService gameJoinService)
    {
        _gameJoinService = gameJoinService;
    }

    public override void Configure()
    {
        Get("api/v1/games/join/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Games));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(await _gameJoinService.Join(userId, gameId), cancellation: ct);
    }
}