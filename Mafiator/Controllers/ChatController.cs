using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
            var chats = await unitOfWork.ChatMessage.GetByRoom(Guid.Parse(roomId));
            chats.ForEach(c=>c.Image=Constants.BlobStorageEndpoint+c.Image);
            return Ok(new ApiResult<List<ChatMessageDto>>()
            {
                IsSuccess = true,
                Data = chats
            });
        }
    }
}
