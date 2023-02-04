using Mafiator.Service.Contracts.Games;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class IsJoined : EndpointWithoutRequest<ApiResult<string>>
{
    private readonly IGameService _gameService;

    public IsJoined(IGameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Get("api/v1/games/isJoined/{gameId}");
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
        await SendMemoryPackAsync(await _gameService.IsJoined(userId, gameId), cancellation: ct);
    }
}