using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Extensions;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Users;

[Authorize]
public class Status : EndpointBaseAsync
    .WithoutRequest
    .WithActionResult<ApiResult<UserStatusResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Status(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/users/status")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Status), Tags = new[] { "Users Endpoints" })]
    public override async Task<ActionResult<ApiResult<UserStatusResult>>> HandleAsync(CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var res = await _unitOfWork.GameMember.GetUserStatusFast(userId);
        var status = new UserStatusResult();
        var totalMafia = res.Count(s => s.GameRole.IsMafia());
        var winMafia = res.Count(s => s.GameRole.IsMafia() && s.GameStatus == GameStatus.MafiaWin);
        status.MafiaWin = totalMafia == 0 ? 0 : winMafia / totalMafia;
        var totalCitizen = res.Count(s => s.GameRole.IsCitizen());
        var winCitizen = res.Count(s => s.GameRole.IsCitizen() && s.GameStatus == GameStatus.CitizenWin);
        status.CitizenWin = totalCitizen == 0 ? 0 : winCitizen / totalCitizen;
        status.TotalWin = winMafia + winCitizen;

        return Ok(new ApiResult<UserStatusResult>()
        {
            IsSuccess = true,
            Data = status
        });
    }
}