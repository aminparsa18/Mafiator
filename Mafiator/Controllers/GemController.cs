using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Api.Controllers
{
    public class GemController:ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        public GemController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new ApiResult<IEnumerable<GemDto>>()
            {
                IsSuccess = true,
                Data = await _unitOfWork.Gem.GetAllDto()
            });
        }
    }
}
