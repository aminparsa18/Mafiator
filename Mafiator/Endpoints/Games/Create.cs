using Ardalis.ApiEndpoints;
using AutoMapper;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Common.Data.Enums;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Games;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<GameCreateRequest>
    .WithActionResult<ApiResult<GameCreateResult>>
{
    private readonly IValidator<GameCreateRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public Create(IValidator<GameCreateRequest> validator, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/games")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Game Endpoints" })]
    public override async Task<ActionResult<ApiResult<GameCreateResult>>> HandleAsync(GameCreateRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var already = await _unitOfWork.Game.IsAlreadyPlaying(request.RoomId.ToString());
        if (!string.IsNullOrEmpty(already))
            return Ok(new ApiResult<GameCreateResult>()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.Conflict,
                Errors = new[] { "Another game is already playing" }
            });
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
        return Ok(new ApiResult<GameCreateResult>()
        {
            Data = new GameCreateResult() { Id = game.Id },
            IsSuccess = true
        });
    }
}