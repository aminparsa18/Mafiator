using AutoMapper;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Entities.Models;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Games;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Games;

public class GameCreateService : IGameCreateService
{
    private readonly IValidator<GameCreateRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public GameCreateService(IValidator<GameCreateRequest> validator, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ApiResult<GameCreateResult>> Create(GameCreateRequest request)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return new ApiResult<GameCreateResult>
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            };
        var already = await _unitOfWork.Game.IsAlreadyPlaying(request.RoomId.ToString());
        if (!string.IsNullOrEmpty(already))
            return new ApiResult<GameCreateResult>()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.Conflict,
                Errors = new[] { "Another game is already playing" }
            };
        var game = _mapper.Map<GameCreateRequest, Game>(request);
        game.Status = GameStatus.NotStarted;
        game.Capacity = (short)request.Roles.Sum(r => r.Count);
        await _unitOfWork.Game.AddFast(game);
        var members = new List<GameMember>();
        foreach (var member in request.Roles)
        {
            for (int i = 0; i < member.Count; i++)
                members.Add(new GameMember()
                {
                    Id = Guid.NewGuid(),
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    Role = member.Role,
                    GameId = game.Id,
                    Status = PlayerStatus.Playing
                });
        }

        await _unitOfWork.GameMember.AddRangeFast(members);
        return new ApiResult<GameCreateResult>
        {
            Data = new GameCreateResult() { Id = game.Id },
            IsSuccess = true
        };
    }
}