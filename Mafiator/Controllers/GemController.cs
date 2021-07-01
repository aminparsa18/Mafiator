using System.Collections.Generic;
using System.Threading.Tasks;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos;
using Mafiator.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Mafiator.Api.Controllers
{
    public class GemController:ApiBaseController
    {
        private readonly IUnitOfWork unitOfWork;
        public GemController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new ApiResult<IEnumerable<GemDto>>()
            {
                IsSuccess = true,
                Data = await unitOfWork.Gem.GetAllDto()
            });
        }
    }
}
