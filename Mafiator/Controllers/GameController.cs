using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Hangfire;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Common.Extensions;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Entities.Enums;
using Mafiator.IocConfig.Hubs;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;

namespace Mafiator.Api.Controllers
{
    [Authorize]
    public class GameController : ApiBaseController
    {
        private readonly IGameService gameService;
        private readonly ILiveEventManager liveEventManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHubContext<GameHub> _gameHub;

        public GameController(ILiveEventManager liveEventManager, IGameService gameService, IUnitOfWork unitOfWork,
            IMapper mapper,
            IHubContext<GameHub> gameHub)
        {
            this.gameService = gameService;
            this.liveEventManager = liveEventManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _gameHub = gameHub;
        }

        [HttpGet]
        public IActionResult Test()
        {
            //for (int i = 1; i < 25; i++)
            //{
            //    var directory = System.IO.Directory.GetFiles(System.IO.Path.Combine(env.ContentRootPath, "Assets/Actor_" + i.ToString("00")));
            //    foreach (var file in directory)
            //    {
            //        AudioToImageConverter.CreateSpectrogram(file);
            //    }
            //}

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] GameCreateDto gameCreateDto)
        {
            var already = await _unitOfWork.Game.IsAlreadyPlaying(gameCreateDto.RoomId.ToString());
            if (!string.IsNullOrEmpty(already))
                return Ok(new ApiResult<GameResultDto>()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.Conflict,
                    Errors = new[] {"Another game is already playing"}
                });
            var game = _mapper.Map<GameCreateDto, Game>(gameCreateDto);
            game.Status = GameStatus.NotStarted;
            game.Capacity = (short) gameCreateDto.Roles.Sum(r => r.Count);
            await _unitOfWork.Game.AddFast(game);
            var members = new List<GameMember>();
            foreach (var member in gameCreateDto.Roles)
            {
                for (int i = 0; i < member.Count; i++)
                    members.Add(new GameMember()
                    {
                        Id = Ulid.NewUlid(),
                        CreatedDate = DateTime.Now,
                        ModifiedDate = DateTime.Now,
                        Role = member.Role,
                        GameId = game.Id,
                        Status = PlayerStatus.Playing
                    });
            }

            await _unitOfWork.GameMember.AddRangeFast(members);
            return Ok(new ApiResult<GameResultDto>()
            {
                Data = new GameResultDto() {Id = game.Id},
                IsSuccess = true
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetByRoom(string roomId)
        {
            var data = await _unitOfWork.Game.GetByRoom(roomId);
            return Ok(new ApiResult<IEnumerable<RoomGameDto>>
            {
                IsSuccess = true,
                Data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailable()
        {
            var data = await _unitOfWork.Game.GetAvailables();
            return Ok(new ApiResult<IEnumerable<GameDto>>
            {
                IsSuccess = true,
                Data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetWaitingGameByRoom(string roomId)
        {
            var data = await _unitOfWork.Game.GetWaitingGameByRoom(Ulid.Parse(roomId));
            data?.Members.ForEach(m => m.Image = Constants.BlobStorageEndpoint + m.Image);
            return Ok(new ApiResult<WaitingGameDto>
            {
                IsSuccess = true,
                Data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetWaitingGameByGame(string gameId)
        {
            var data = await _unitOfWork.Game.GetWaitingGameByGame(Ulid.Parse(gameId));
            data?.Members.ForEach(m => m.Image = Constants.BlobStorageEndpoint + m.Image);
            return Ok(new ApiResult<WaitingGameDto>
            {
                IsSuccess = true,
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> Join([FromBody] string gameId)
        {
            var members = await _unitOfWork.GameMember.GetUsersByGame(gameId);
            if (!members.Any())
                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    Errors = new[] {"No such game!!!"},
                    StatusCode = ApiResultStatusCode.NotFound
                });
            if (!members.Any(m => string.IsNullOrEmpty(m.UserId)))
            {
                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    Errors = new[] {"Sorry,no more capacity for this game"},
                    StatusCode = ApiResultStatusCode.BadRequest
                });
            }

            var userId = User.FindFirstValue(ClaimTypes.Name);
            if (members.Any(m => m.UserId == userId))
                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    Errors = new[]
                        {"You are already part of this game!!!", userId, JsonConvert.SerializeObject(members)},
                    StatusCode = ApiResultStatusCode.Conflict
                });
            var selected = members.Where(m => string.IsNullOrEmpty(m.UserId)).SelectRandom();
            await _unitOfWork.GameMember.Join(userId, selected.MemberId);
            await _gameHub.Clients.Group(gameId).SendAsync("Join", userId);
            if (members.Count(m => string.IsNullOrEmpty(m.UserId)) == 1)
            {
                await _gameHub.Clients.Group(gameId).SendAsync("StartLiveEvent");
                var live= await liveEventManager.CreateLiveEvent(gameId);
                await _gameHub.Clients.Group(gameId).SendAsync("StartGame",live.Item1,live.Item2);
                BackgroundJob.Schedule(() => gameService.SetTurn(gameId, 0), TimeSpan.FromSeconds(30));
                await _unitOfWork.Game.StartGame(gameId);
            }

            return Ok(new ApiResult()
            {
                IsSuccess = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Leave([FromBody] string gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var member = await _unitOfWork.GameMember.GetUser(userId, gameId);
            if (!member.Any())
                return Ok(new ApiResult<string>()
                {
                    IsSuccess = false,
                    Errors = new[] {"You are not member of this game"},
                    StatusCode = ApiResultStatusCode.NotFound
                });
            await _unitOfWork.GameMember.Leave(userId);
            await _gameHub.Clients.Group(gameId).SendAsync("Join", userId.ToString());
            return Ok(new ApiResult<string>()
            {
                IsSuccess = true,
            });
        }

        [HttpGet]
        public async Task<IActionResult> IsJoined(string gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var member = await _unitOfWork.Game.IsJoinedFast(userId, gameId);
            return Ok(new ApiResult<string>()
            {
                IsSuccess = true,
                Data = member
            });
        }

        [HttpGet]
        public IActionResult GetAllRoles()
        {
            var roles = new List<GameRole>()
            {
                GameRole.Mafia,
                GameRole.Citizen,
                GameRole.GodFather,
                GameRole.Terrorist,
                GameRole.Detective,
                GameRole.Doctor,
                GameRole.Sniper,
                GameRole.Gun,
                GameRole.Healer,
                GameRole.Immortal,
                GameRole.Natasha,
                GameRole.Priest,
                GameRole.Judge
            };
            return Ok(new ApiResult<List<GameRole>>()
            {
                Data = roles,
                IsSuccess = true
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleOfPlayer(string gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var role = await _unitOfWork.GameMember.GetRoleOfPlayer(userId, gameId);
            if (role.Any())
                return Ok(new ApiResult<PlayerRoleDto>()
                {
                    IsSuccess = true,
                    Data = role.FirstOrDefault()
                });
            return Ok(new ApiResult<PlayerRoleDto>()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] {"No such member found for this game"}
            });
        }

        //if mafia
        [HttpGet]
        public async Task<IActionResult> GetMafiaPartners(string gameId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var roles = await _unitOfWork.GameMember.GetRoleOfPlayer(userId, gameId);
            if (!roles.Any())
                return Ok(new ApiResult<PlayerRoleDto>()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.NotFound,
                    Errors = new[] {"No such member found for this game"}
                });
            var role = roles.FirstOrDefault();
            if (role.Role != GameRole.GodFather && role.Role != GameRole.Mafia)
                return Ok(new ApiResult<PlayerRoleDto>()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.NotFound,
                    Errors = new[] {"Reallyyy???!!!Only mafia players can use it"}
                });
            var partners = await _unitOfWork.GameMember.GetMafiaPartners(role.MemberId, gameId);

            return Ok(new ApiResult<IEnumerable<PlayerRoleDto>>()
            {
                IsSuccess = true,
                Data = partners
            });
        }
    }
}