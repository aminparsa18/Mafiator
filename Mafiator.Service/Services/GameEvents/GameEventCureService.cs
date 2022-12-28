using AutoMapper;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Enums;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities.Models;
using Mafiator.Repository;
using Mafiator.Service.Contracts.GameEvents;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.GameEvents;

public class GameEventCureService : IGameEventCureService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GameEventCureService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult> Cure(string userId, GameEventRequest request)
    {
        var playerStatus = await _unitOfWork.GameMember.GetUserStatusFast(userId);
        if (!playerStatus.Any())
            return new ApiResult
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "Seems you are not part of this game!!!" }
            };

        if (playerStatus.FirstOrDefault().GameRole == GameRole.Doctor)
        {
            var cures = await _unitOfWork.GameEvent.GetByGame(request.GameId.ToString());
            if (cures.Count(c => c.EventType == GameEventType.Cured) < 2)
            {
                //if already saved himself don't allow again
                if (cures.Any(c =>
                    c.EventType == GameEventType.Cured && c.MemberId == playerStatus.FirstOrDefault().MemberId))
                {
                    return new ApiResult
                    {
                        IsSuccess = false,
                        StatusCode = ApiResultStatusCode.NotFound,
                        Errors = new[] { "Can't cure yourself more than once" }
                    };
                }

                var gameEvent = _mapper.Map<GameEvent>(request);
                await _unitOfWork.GameEvent.AddFast(gameEvent);
                return new ApiResult
                {
                    IsSuccess = true
                };
            }

            return new ApiResult
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "max limit of cure has reached" }
            };
        }

        return new ApiResult
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.Conflict,
            Errors = new[] { "only doctor have rights to do this!!!" }
        };
    }
}