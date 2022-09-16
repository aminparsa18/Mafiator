using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
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
    [OpenApiOperation("GameEvents.GetNughtResult", "", "Get results of night in a game.")]
    [OpenApiTag("Game Events Endpoints")]
    public override ActionResult<ApiResult<IEnumerable<GameEventResult>>> Handle(string gameId)
    {
        return Ok(new ApiResult<IEnumerable<GameEventResult>>()
        {
            IsSuccess = true,
            Data = _cache.GetCache<List<GameEventResult>>($"NightResults-{gameId}")
        });
    }
}