using Ardalis.ApiEndpoints;
using AutoMapper;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.GameEvents;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<GameEventRequest>
    .WithActionResult<ApiResult>
{
    private readonly IValidator<GameEventRequest> _validator;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public Create(IValidator<GameEventRequest> validator, IMapper mapper, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/gameevents")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Game Events Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(GameEventRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        var gameEvent = _mapper.Map<GameEvent>(request);
        await _unitOfWork.GameEvent.AddFast(gameEvent);
        return Ok();
    }
}