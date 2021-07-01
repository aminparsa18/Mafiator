using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    public class AvatarController:ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public AvatarController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var avatars = await _unitOfWork.Avatar.GetAllDto();
            //  avatars.ForAll(s=>s.Name=Constants.BlobStorageEndpoint+s.Name);
            return Ok(new ApiResult<IEnumerable<AvatarDto>>
            {
              Data = avatars,
              IsSuccess = true
            });
        }
        [HttpGet]
        public IActionResult RemoveCache()
        {
            CacheFactory.GetCache().Remove("ActiveAvatars");
            return Ok();
        }
    }
}
