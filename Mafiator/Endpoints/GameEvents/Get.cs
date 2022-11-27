using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
public class Get : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<GameEventResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gameevents/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Get), Tags = new[] { "Game Event Endpoints" })]
    public override async Task<ActionResult<ApiResult<GameEventResult>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResult<IEnumerable<GameEventResult>>()
        {
            IsSuccess = true,
            Data = await _unitOfWork.GameEvent.GetByGame(gameId)
        });
    }
}