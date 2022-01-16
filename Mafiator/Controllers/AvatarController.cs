using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Mafiator.Service.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Api.Controllers
{
    public class AvatarController:ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILiveEventManager _liveEventManager;
        public AvatarController(IUnitOfWork unitOfWork,ILiveEventManager liveEventManager)
        {
            _unitOfWork = unitOfWork;
            _liveEventManager = liveEventManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var avatars = await _unitOfWork.Avatar.GetAllDto();
            foreach (var avatar in avatars)
            {
                avatar.Name = Constants.BlobStorageEndpoint + avatar.Name;
            }
            return Ok(new ApiResult<IEnumerable<AvatarDto>>
            {
              Data = avatars,
              IsSuccess = true
            });
        }

        [HttpGet]
        public async Task<IActionResult> TestSetting()
        {
            await _liveEventManager.CreateLiveEvent(Guid.NewGuid().ToString());
            return Ok();
        }
        [HttpGet]
        public IActionResult RemoveCache()
        {
            CacheFactory.GetCache().Remove("ActiveAvatars");
            return Ok();
        }
    }
}
