using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

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
    [OpenApiOperation("Games.GetAvailables", "", "Retrieves available games to play.")]
    [OpenApiTag("Games Endpoints")]
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