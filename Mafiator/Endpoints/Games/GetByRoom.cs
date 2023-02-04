using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Games;

public class GetByRoom : EndpointWithoutRequest<ApiResult<IEnumerable<RoomGameResult>>>
{
    private readonly IGameService _gameService;

    public GetByRoom(IGameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Get("api/v1/games/{roomId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Games));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string roomId = Route<string>("roomId");
        await SendMemoryPackAsync(await _gameService.GetByRoom(roomId), cancellation: ct);
    }
}