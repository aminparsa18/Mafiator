using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetAvailables : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<IEnumerable<AvailableGameResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAvailables(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/availables")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetAvailables), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<AvailableGameResult>>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.Game.GetAvailables();
        return Ok(new ApiResult<IEnumerable<AvailableGameResult>>
        {
            IsSuccess = true,
            Data = data
        });
    }
}