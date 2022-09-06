using AutoMapper;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Data.Dtos.Room;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mafiator.Api.Controllers
{
    [Authorize(Roles = "Player")]
    public class RoomController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(int skip)
        {
            var data = await _unitOfWork.Room.GetDtoPageFast(skip);
            return Ok(new ApiResult<IEnumerable<RoomDetailsResult>>()
            {
                IsSuccess = true,
                Data = data
            });
        }

        //[HttpPut]
        //public async Task<IActionResult> UpdateRoom([FromBody] RoomJoinDto roomDto)
        //{
        //    var room = await _unitOfWork.Room.Get(roomDto.RoomId);
        //    await _unitOfWork.Room.UpdateFast(room);
        //    return Ok(new ApiResult()
        //    {
        //        IsSuccess = true
        //    });
        //}

        [HttpGet]
        public async Task<IActionResult> IsJoined(string roomId)
        {
            var userId = User.FindFirstValue(ClaimTypes.Name);
            var member=await _unitOfWork.Room.IsJoinedFast(userId, roomId);
            return Ok(new ApiResult<string>()
            {
                IsSuccess = true,
                Data = member
            });
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> UpdateRoomImage([FromBody] UpdateRoomImageDto roomImage)
        {
            var room = await _unitOfWork.Room.Get(roomImage.RoomId);
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
            if (room.UserId != userId)
                return BadRequest(new ApiResult()
                {
                    IsSuccess = false,
                    Errors = new[] {"این اتاق به شما تعلق ندارد"}
                });
            _unitOfWork.Room.Update(room);
            await _unitOfWork.Commit();
            return Ok(new ApiResult()
            {
                IsSuccess = true
            });
        }
    }
}