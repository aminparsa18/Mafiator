using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;
using System;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetAppointedDetails : EndpointWithoutRequest<ApiResult<AppointedGameResult>>
{
    private readonly IGameService _gameService;

    public GetAppointedDetails(IGameService gameService)
    {
        _gameService = gameService;
    }

    public override void Configure()
    {
        Get("api/v1/games/appointed-details/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.Games));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(await _gameService.GetAppointedDetails(Guid.Parse(gameId)), cancellation: ct);
    }
}