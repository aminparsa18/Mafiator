using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Votes;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Votes;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<VoteCreateRequest>
    .WithActionResult<ApiResult>
{
    private readonly IValidator<VoteCreateRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public Create(IUnitOfWork unitOfWork, IValidator<VoteCreateRequest> validator)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/votes")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(Create), Tags = new[] { "Votes Endpoints" })]
    public override async Task<ActionResult<ApiResult>> HandleAsync(VoteCreateRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        await _unitOfWork.Vote.AddRangeFast(request.Targets.Select(s => new Vote()
        {
            Id = Guid.NewGuid(),
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now,
            GameId = request.GameId,
            TargetId = s,
            VoterId = request.VoterId
        }));
        return Ok(new ApiResult
        {
            IsSuccess = true
        });
    }
}