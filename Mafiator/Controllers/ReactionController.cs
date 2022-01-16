using AutoMapper;
using Mafiator.Api.Controllers.Base;
using Mafiator.Common.Api;
using Mafiator.Data.Dtos.Game;
using Mafiator.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Api.Controllers
{
    public class ReactionController : ApiBaseController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReactionController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new ApiResult<IEnumerable<ReactionDto>>()
            {
                IsSuccess = true,
                Data = await _unitOfWork.Reaction.GetAllDtos()
            });
        }
    }
}
