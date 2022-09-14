using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameMembers;

public class GetPlayerRole : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<PlayerRoleResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetPlayerRole(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/role/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("GameMembers.GetPlayerRole", "", "Retrieves role of game player.")]
    [OpenApiTag("GameMembers Endpoints")]
    public override async Task<ActionResult<ApiResult<PlayerRoleResult>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var role = await _unitOfWork.GameMember.GetRoleOfPlayer(userId, gameId);
        if (role.Any())
            return Ok(new ApiResult<PlayerRoleResult>()
            {
                IsSuccess = true,
                Data = role.FirstOrDefault()
            });
        return Ok(new ApiResult<PlayerRoleResult>()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.NotFound,
            Errors = new[] { "No such member found for this game" }
        });
    }
}