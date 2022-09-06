using AutoMapper;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Common.Enums;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mafiator.Api.Controllers
{
    public class GameEventController : ApiBaseController
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;

        public GameEventController(IMapper mapper,IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            this._mapper = mapper;
            this._cache = memoryCache;
            this._unitOfWork = unitOfWork;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GameEventDto ev)
        {
            var gameEvent = _mapper.Map<GameEvent>(ev);
            await _unitOfWork.GameEvent.AddFast(gameEvent);
            return Ok();
        }

        
        [HttpPost]
        public async Task<IActionResult> Cure([FromBody] GameEventDto ev)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var playerStatus = await _unitOfWork.GameMember.GetUserStatusFast(userId);
            if (playerStatus.Any())
            {
                if (playerStatus.FirstOrDefault().GameRole == GameRole.Doctor)
                {
                    var cures = await _unitOfWork.GameEvent.GetByGame(ev.GameId.ToString());
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
                                Errors = new[] {"Can't cure yourself more than once"}
                            });
                        }

                        var gameEvent = _mapper.Map<GameEvent>(ev);
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
                        Errors = new[] {"max limit of cure has reached"}
                    });
                }

                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.Conflict,
                    Errors = new[] {"only doctor have rights to do this!!!"}
                });
            }

            return Ok(new ApiResult()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] {"Seems you are not part of this game!!!"}
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetStatus(string gameId)
        {

            return Ok(new ApiResult<IEnumerable<GameEventStatusDto>>()
            {
                IsSuccess = true,
                Data = await _unitOfWork.GameEvent.GetByGame(gameId)
            });
        }

        [HttpGet]
        public IActionResult GetNightResult(string gameId)
        {
            return Ok(new ApiResult<IEnumerable<GameEventResultDto>>()
            {
                IsSuccess = true,
                Data = _cache.GetCache<List<GameEventResultDto>>($"NightResults-{gameId}")
            });
        }
    }
}