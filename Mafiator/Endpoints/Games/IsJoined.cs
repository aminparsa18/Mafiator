using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class IsJoined : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<string>>
{
    private readonly IUnitOfWork _unitOfWork;

    public IsJoined(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/isJoined/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(IsJoined), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<string>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var member = await _unitOfWork.Game.IsJoinedFast(userId, gameId);
        return Ok(new ApiResult<string>()
        {
            IsSuccess = true,
            Data = member
        });
    }
}