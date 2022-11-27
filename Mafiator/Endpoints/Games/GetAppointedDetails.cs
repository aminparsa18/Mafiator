using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
public class GetAppointedDetails : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<AppointedGameResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetAppointedDetails(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/games/appointed-details/{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetAppointedDetails), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<AppointedGameResult>>> HandleAsync(string gameId, CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.Game.GetWaitingGameInformations(Guid.Parse(gameId));
        data?.Members.ForEach(m => m.Image = Data.Constants.BlobStorageEndpoint + m.Image);
        return Ok(new ApiResult<AppointedGameResult>
        {
            IsSuccess = true,
            Data = data
        });
    }
}
