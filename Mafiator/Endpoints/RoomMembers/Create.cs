using Ardalis.ApiEndpoints;
using FluentValidation;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using Mafiator.Entities;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.RoomMembers;

[Authorize]
[Produces("application/x-msgpack")]
public class Create : EndpointBaseAsync
    .WithRequest<NewMembersRequest>
    .WithActionResult<ApiResult>
{
    private readonly IValidator<NewMembersRequest> _validator;
    private readonly IUnitOfWork _unitOfWork;

    public Create(IValidator<NewMembersRequest> validator, IUnitOfWork unitOfWork)
    {
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpPost("api/v{version:apiVersion}/roommembers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("RoomMembers.Create", "", "Adds users into room as room members.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult>> HandleAsync(NewMembersRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request);
        if (!validationResult.IsValid)
            return Ok(new ApiResult
            {
                StatusCode = ApiResultStatusCode.BadRequest,
                Errors = validationResult.Errors.Select(e => e.ErrorMessage)
            });
        foreach (var userId in request.Users)
        {
            var user = await _unitOfWork.RoomMember.FindInRoom(request.RoomId, userId);
            if (!user.Any())
            {
                await _unitOfWork.RoomMember.AddFast(new RoomMember()
                {
                    UserId = userId,
                    RoomId = request.RoomId,
                });
            }
        }
        return Ok(new ApiResult()
        {
            IsSuccess = true,
        });
    }
}