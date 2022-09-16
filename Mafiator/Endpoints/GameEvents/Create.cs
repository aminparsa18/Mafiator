using Ardalis.ApiEndpoints;
using AutoMapper;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<GameEventRequest>
    .WithActionResult<ApiResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public Create(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/gameevents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("GameEvents.Create", "", "Creates a new game event.")]
    [OpenApiTag("Game Events Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(GameEventRequest request, CancellationToken cancellationToken = default)
    {
        var gameEvent = _mapper.Map<GameEvent>(request);
        await _unitOfWork.GameEvent.AddFast(gameEvent);
        return Ok();
    }
}