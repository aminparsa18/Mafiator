using AutoMapper;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities.Models;
using Mafiator.Repository;
using Mafiator.Service.Contracts.GameEvents;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.GameEvents;

public class GameEventCreateService : IGameEventCreateService
{
    private readonly IValidator<GameEventRequest> _validator;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public GameEventCreateService(IValidator<GameEventRequest> validator, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult> Create(GameEventRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };

        var gameEvent = _mapper.Map<GameEvent>(request);
        await _unitOfWork.GameEvent.AddFast(gameEvent);
        return new ApiResult { IsSuccess = true };
    }
}