using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Entities.Enums;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    public class GameEventController : ApiBaseController
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMemoryCache cache;

        public GameEventController(IMapper mapper,IMemoryCache memoryCache, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.cache = memoryCache;
            this.unitOfWork = unitOfWork;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GameEventDto ev)
        {
            var gameEvent = mapper.Map<GameEvent>(ev);
            await unitOfWork.GameEvent.AddFast(gameEvent);
            return Ok();
        }

        
        [HttpPost]
        public async Task<IActionResult> Cure([FromBody] GameEventDto ev)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var playerStatus = await unitOfWork.GameMember.GetUserStatusFast(userId);
            if (playerStatus.Any())
            {
                if (playerStatus.FirstOrDefault().GameRole == GameRole.Doctor)
                {
                    var cures = await unitOfWork.GameEvent.GetByGame(ev.GameId.ToString());
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

                        var gameEvent = mapper.Map<GameEvent>(ev);
                        await unitOfWork.GameEvent.AddFast(gameEvent);
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
                Data = await unitOfWork.GameEvent.GetByGame(gameId)
            });
        }

        [HttpGet]
        public IActionResult GetNightResult(string gameId)
        {
            return Ok(new ApiResult<IEnumerable<GameEventResultDto>>()
            {
                IsSuccess = true,
                Data = cache.GetCache<List<GameEventResultDto>>($"NightResults-{gameId}")
            });
        }
    }
}