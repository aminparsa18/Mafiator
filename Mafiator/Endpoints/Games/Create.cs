using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Service.Contracts.Games;

namespace Mafiator.Api.Endpoints.Games;

public class Create : Endpoint<GameCreateRequest, ApiResult<GameCreateResult>>
{
    private readonly IGameCreateService _gameCreateService;

    public Create(IGameCreateService gameCreateService)
    {
        _gameCreateService = gameCreateService;
    }

    public override void Configure()
    {
        Post("api/v1/games");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d =>d.Produces(200).WithTags(EndpointsTags.Games));
    }

    public override async Task HandleAsync(GameCreateRequest request, CancellationToken ct) =>
        await SendMemoryPackAsync(await _gameCreateService.Create(request), cancellation: ct);
}