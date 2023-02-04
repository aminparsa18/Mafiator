using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameEvents;

public class GetNightResult : EndpointWithoutRequest<ApiResult<IEnumerable<GameEventResult>>>
{
    private readonly IMemoryCache _cache;

    public GetNightResult(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }

    public override void Configure()
    {
        Get("api/v1/gameevents/night/{gameId}");
        Summary(s =>
        {
            s.Summary = "sadasdas";
            s.Description = "desxvxcvxv";
        });
        Description(d =>
        d.Produces(200).WithTags(EndpointsTags.GameEvents));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string gameId = Route<string>("gameId");
        await SendMemoryPackAsync(new ApiResult<IEnumerable<GameEventResult>>()
        {
            IsSuccess = true,
            Data = _cache.GetCache<List<GameEventResult>>($"NightResults-{gameId}")
        }, cancellation: ct);
    }
}