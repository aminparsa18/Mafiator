using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    public class VoteController:ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public VoteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<IActionResult> AddVotes([FromBody] VoteDto dto)
        {
            await _unitOfWork.Vote.AddRangeFast(dto.Targets.Select(s => new Vote()
            {
                Id = Ulid.NewUlid(),
                CreatedDate = DateTime.Now,
                ModifiedDate = DateTime.Now,
                GameId = dto.GameId,
                TargetId = s,
                VoterId = dto.VoterId
            }));

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> GetVoteStatus(string gameId)
        {
            return Ok(new ApiResult<IEnumerable<VoteStatusDto>>()
            {
                Data = await _unitOfWork.Vote.GetVoteStatus(gameId),
                IsSuccess = true
            });
        }
    }
}
