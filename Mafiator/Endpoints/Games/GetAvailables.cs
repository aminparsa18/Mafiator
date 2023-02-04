using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.Games;

public class GetAvailables : EndpointWithoutRequest<ApiResult<IEnumerable<AvailableGameResult>>>
{
    private readonly IGameService _gameService;

    public GetAvailables(IGameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Get("api/v1/games/availables");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Games));
    }

    public override async Task HandleAsync(CancellationToken ct) =>
        await SendMemoryPackAsync(await _gameService.GetAvailable(), cancellation: ct);
}