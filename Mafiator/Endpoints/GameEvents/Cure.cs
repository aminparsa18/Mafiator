using Ardalis.ApiEndpoints;
using AutoMapper;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
[Produces("application/x-msgpack")]
public class Cure : EndpointBaseAsync
    .WithRequest<GameEventRequest>
    .WithActionResult<ApiResult>
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public Cure(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/gameevents/cure")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("GameEvents.Cure", "", "Cures a player by doctor.")]
    [OpenApiTag("Game Events Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(GameEventRequest request, CancellationToken cancellationToken = default)
    {
        var userId = User.FindFirstValue(ClaimTypes.Name);
        var playerStatus = await _unitOfWork.GameMember.GetUserStatusFast(userId);
        if (playerStatus.Any())
        {
            if (playerStatus.FirstOrDefault().GameRole == GameRole.Doctor)
            {
                var cures = await _unitOfWork.GameEvent.GetByGame(request.GameId.ToString());
                if (cures.Count(c => c.EventType == GameEventType.Cured) < 2)
                {
                    //if already saved himself don't allow again
                    if (cures.Any(c =>
                        c.EventType == GameEventType.Cured && c.MemberId == playerStatus.FirstOrDefault().MemberId))
                    {
                        return Ok(new ApiResult()
                        {
                            IsSuccess = false,
                            StatusCode = ApiResultStatusCode.NotFound,
                            Errors = new[] { "Can't cure yourself more than once" }
                        });
                    }

                    var gameEvent = _mapper.Map<GameEvent>(request);
                    await _unitOfWork.GameEvent.AddFast(gameEvent);
                    return Ok(new ApiResult()
                    {
                        IsSuccess = true
                    });
                }

                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.NotFound,
                    Errors = new[] { "max limit of cure has reached" }
                });
            }

            return Ok(new ApiResult()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.Conflict,
                Errors = new[] { "only doctor have rights to do this!!!" }
            });
        }

        return Ok(new ApiResult()
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.NotFound,
            Errors = new[] { "Seems you are not part of this game!!!" }
        });
    }
}