using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{ 
    [Authorize]
    public class RoomMemberController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        public RoomMemberController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetByRoom(string roomId)
        {
            var data = await _unitOfWork.RoomMember.GetByRoomFast(roomId);
            foreach (var item in data)
            {
                item.Level = (item.TotalGame / 10) + 1;
                item.Image = Constants.BlobStorageEndpoint + item.Image;
            }

            return Ok(new ApiResult<IEnumerable<RoomMemberDto>>()
            {
                Data = data,
                IsSuccess = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> AddMember([FromBody] AddMemberDto member)
        {
            foreach (var userId in member.Users)
            {
                var user = await _unitOfWork.RoomMember.FindInRoom(member.RoomId, userId);
                if (!user.Any())
                {
                    await _unitOfWork.RoomMember.AddFast(new RoomMember()
                    {
                        UserId = userId,
                        RoomId = member.RoomId,
                    });
                }
            }
            return Ok(new ApiResult()
            {
                IsSuccess = true,
            });
        }

        [HttpPost]
        public async Task<IActionResult> JoinRoom([FromBody] Ulid roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            await _unitOfWork.RoomMember.AddFast(new RoomMember()
            {
                UserId = Ulid.Parse(userId),
                RoomId = roomId
            });
            return Ok(new ApiResult()
            {
                IsSuccess = true
            });
        }

        [HttpPost]
        public async Task<IActionResult> Leave([FromBody] string roomId)
        {
            var userId = Ulid.Parse(User.FindFirstValue(ClaimTypes.Name));
            var members = await _unitOfWork.RoomMember.GetFast(r => r.UserId == userId && r.RoomId == Ulid.Parse(roomId));
            if (!members.Any())
                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    Errors = new[] { "You are not member of this room" }
                });
            await _unitOfWork.RoomMember.RemoveFast(members.FirstOrDefault());
            return Ok(new ApiResult()
            {
                IsSuccess = true
            });
        }
    }
}