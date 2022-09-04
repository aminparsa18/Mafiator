using AutoMapper;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Common.Helpers;
using Mafiator.Data.Dtos.Room;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> GetRoom(string roomId)
        {
            var data = await _unitOfWork.Room.GetRoomFast(roomId);
            if (!data.Any())
            {
                return Ok(new ApiResult()
                {
                    IsSuccess = false,
                    StatusCode = ApiResultStatusCode.NotFound,
                    Errors = new[] {"Room not found"}
                });
            }

            return Ok(new ApiResult<RoomDto>()
            {
                IsSuccess = true,
                Data = data.FirstOrDefault()
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetMyRooms()
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
            var data = await _unitOfWork.Room.GetMyRoomsFast(userId);
            return Ok(new ApiResult<IEnumerable<RoomDto>>()
            {
                IsSuccess = true,
                Data = data
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetPage(int skip)
        {
            var data = await _unitOfWork.Room.GetDtoPageFast(skip);
            return Ok(new ApiResult<IEnumerable<RoomDto>>()
            {
                IsSuccess = true,
                Data = data
            });
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] RoomCreateDto roomCreateDto)
        {
            var room = _mapper.Map<RoomCreateDto, Room>(roomCreateDto);
            room.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
            room.Code = RandomHelper.CreateRandomText(8);
            var roomId = await _unitOfWork.Room.AddFast(room);
            await _unitOfWork.RoomMember.AddFast(new RoomMember()
            {
                RoomId = Guid.Parse(roomId.ToString()),
                UserId = room.UserId,
            });
            foreach (var user in roomCreateDto.Users)
            {
                await _unitOfWork.RoomMember.AddFast(new RoomMember()
                {
                    RoomId = Guid.Parse(roomId.ToString()),
                    UserId = user,
                });
            }
            return Ok(new ApiResult<RoomIdDto>()
            {
                IsSuccess = true,
                Data = new RoomIdDto() {Id = room.Id}
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

        [HttpPost]
        public async Task<IActionResult> Join([FromBody] string code)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
            var roomId = await _unitOfWork.Room.GetByCode(code);
            if (string.IsNullOrEmpty(roomId))
            {
                return Ok(new ApiResult<string>()
                {
                    IsSuccess = false,
                    Errors = new []{"No such room exist"},
                    StatusCode = ApiResultStatusCode.NotFound
                });
            }
            var member = await _unitOfWork.Room.IsJoinedFast(userId.ToString(), roomId);
            if(!string.IsNullOrEmpty(member))
                return Ok(new ApiResult<string>()
                {
                    IsSuccess = false,
                    Errors = new[] { "You are already joined" },
                    StatusCode = ApiResultStatusCode.Conflict,
                    Data = roomId
                });
            await _unitOfWork.RoomMember.AddFast(new RoomMember()
            {
                RoomId = Guid.Parse(roomId),
                UserId = userId
            });
            return Ok(new ApiResult<string>()
            {
                IsSuccess = true,
                Data = roomId
            });
        }

        [HttpPost]
        public async Task<IActionResult> Leave([FromBody] string code)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.Name));
            var roomId = await _unitOfWork.Room.GetByCode(code);
            if (string.IsNullOrEmpty(roomId))
            {
                return Ok(new ApiResult<string>()
                {
                    IsSuccess = false,
                    Errors = new[] { "No such room exist" },
                    StatusCode = ApiResultStatusCode.NotFound
                });
            }
            var member = await _unitOfWork.RoomMember.Get(r => r.UserId == userId && r.RoomId == Guid.Parse(roomId));
            if (member==null)
                return Ok(new ApiResult<string>()
                {
                    IsSuccess = false,
                    Errors = new[] { "You are not member of this room" },
                    StatusCode = ApiResultStatusCode.Conflict,
                    Data = roomId
                });
            _unitOfWork.RoomMember.Remove(member);
            await _unitOfWork.Commit();
            return Ok(new ApiResult<string>()
            {
                IsSuccess = true,
                Data = roomId
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