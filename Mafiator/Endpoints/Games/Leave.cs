using Mafiator.Service.Contracts.Games;
using System.Security.Claims;

namespace Mafiator.Api.Endpoints.Games;

public class Leave : EndpointWithoutRequest<ApiResult>
{
    private readonly IGameLeaveService _gameLeaveService;

    public Leave(IGameLeaveService gameLeaveService)
    {
        _gameLeaveService = gameLeaveService;
    }

    public override void Configure()
    {
        Post("api/v1/games/leave/{gameId}");
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
        await SendMemoryPackAsync(await _gameLeaveService.Leave(userId, gameId), cancellation: ct);
    }
}