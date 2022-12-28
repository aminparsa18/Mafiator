using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
public class GetNightResult : EndpointBaseSync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<GameEventResult>>>
{
    private readonly IMemoryCache _cache;

    public GetNightResult(IMemoryCache memoryCache)
    {
        _cache = memoryCache;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gameevents/night/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Game Events Endpoints" })]
    public override ActionResult<ApiResult<IEnumerable<GameEventResult>>> Handle(string gameId)
    {
        return Ok(new ApiResult<IEnumerable<GameEventResult>>()
        {
            IsSuccess = true,
            Data = _cache.GetCache<List<GameEventResult>>($"NightResults-{gameId}")
        });
    }
}