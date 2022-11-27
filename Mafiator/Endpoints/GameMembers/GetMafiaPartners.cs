using Ardalis.ApiEndpoints;
using Azure.ResourceManager.Resources.Models;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameMembers;

[Authorize]
public class GetMafiaPartners : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<IEnumerable<PlayerRoleResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetMafiaPartners(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/gamemembers/mafia-partners/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetMafiaPartners), Tags = new[] { "Game members Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<PlayerRoleResult>>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var roles = await _unitOfWork.GameMember.GetRoleOfPlayer(userId, gameId);
        if (!roles.Any())
            return Ok(new ApiResult<PlayerRoleResult>()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "No such member found for this game" }
            });
        var role = roles.FirstOrDefault();
        if (role.Role != GameRole.GodFather && role.Role != GameRole.Mafia)
            return Ok(new ApiResult<PlayerRoleResult>()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "Reallyyy???!!!Only mafia players can use it" }
            });
        var partners = await _unitOfWork.GameMember.GetMafiaPartners(role.MemberId, gameId);

        return Ok(new ApiResult<IEnumerable<PlayerRoleResult>>()
        {
            IsSuccess = true,
            Data = partners
        });
    }
}