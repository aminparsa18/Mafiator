using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities.Enums;
using Mafiator.Entities.Extensions;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
   // [Authorize]
    public class GameMemberController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        public GameMemberController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetByGame(string gameId)
        {
            var members = await unitOfWork.GameMember.GetByGameFast(gameId);
            foreach (var gameMemberDto in members)
            {
                gameMemberDto.Image = Constants.BlobStorageEndpoint + gameMemberDto.Image;
            }
            return Ok(new ApiResult<IEnumerable<GameMemberDto>>()
            {
                IsSuccess = true,
                Data =  members
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetWaitingPlayersByGame(string gameId)
        {
            var members = await unitOfWork.GameMember.GetWaitingPlayersByGame(gameId);
            foreach (var gameMemberDto in members)
            {
                gameMemberDto.Image = Constants.BlobStorageEndpoint + gameMemberDto.Image;
            }
            return Ok(new ApiResult<IEnumerable<WaitingPlayerDto>>()
            {
                IsSuccess = true,
                Data = members
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetMemberStatus()
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var res = await unitOfWork.GameMember.GetUserStatusFast(userId);
            var status=new UserStatusDto();
                var totalMafia = res.Count(s => s.GameRole.IsMafia());
                var winMafia = res.Count(s => s.GameRole.IsMafia() && s.GameStatus == GameStatus.MafiaWin);
                status.MafiaWin= totalMafia==0 ? 0 : winMafia / totalMafia;
                var totalCitizen = res.Count(s => s.GameRole.IsCitizen());
                var winCitizen = res.Count(s => s.GameRole.IsCitizen() && s.GameStatus == GameStatus.CitizenWin);
                status.CitizenWin = totalCitizen == 0 ? 0 : winCitizen / totalCitizen;
                status.TotalWin = winMafia + winCitizen;
                
            return Ok(new ApiResult<UserStatusDto>()
            {
                IsSuccess = true,
                Data = status
            });
        }
    }
}