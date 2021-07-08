using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    [Authorize]
    public class ChatController : ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;

        public ChatController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetByRoom(string roomId)
        {
            return Ok(new ApiResult<List<ChatMessageDto>>()
            {
                IsSuccess = true,
                Data = await unitOfWork.ChatMessage.GetByRoom(Ulid.Parse(roomId))
            });
        }
    }
}
