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
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    public class GameEventController : ApiBaseController
    {
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;

        public GameEventController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
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
        public async Task<IActionResult> Inquiry([FromBody] GameEventDto ev)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var playerStatus = await unitOfWork.GameMember.GetUserStatusFast(userId);
            if (playerStatus.Any())
            {
                if (playerStatus.FirstOrDefault().GameRole == GameRole.Detective)
                {
                    var targets = await unitOfWork.GameMember.GetPlayerStatusFast(ev.MemberId.ToString());
                    if (targets.Any())
                    {
                        var target = targets.FirstOrDefault();
                        if (target.GameRole == GameRole.Mafia || target.GameRole == GameRole.Terrorist)
                            return Ok(new ApiResult<InquiryStatusDto>()
                            {
                                IsSuccess = true,
                                Data = new InquiryStatusDto() {IsMafia = true}
                            });
                        return Ok(new ApiResult<InquiryStatusDto>()
                        {
                            IsSuccess = true,
                            Data = new InquiryStatusDto() {IsMafia = false}
                        });
                    }

                    return Ok(new ApiResult()
                    {
                        IsSuccess = false,
                        StatusCode = ApiResultStatusCode.NotFound,
                        Errors = new[] {"no such target in game!!!"}
                    });
                }

                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.Conflict,
                    Errors = new[] {"only detective have rights to do this!!!"}
                });
            }

            return Ok(new ApiResult()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] {"no such user in game!!!"}
            });
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
    }
}