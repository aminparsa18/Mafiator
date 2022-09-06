using Ardalis.ApiEndpoints;
using Mafiator.Common.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.Rooms;

public class Get : EndpointBaseAsync
    .WithRequest<string>
    .WithActionResult<ApiResult<RoomDetailsResult>>
{
    private readonly IUnitOfWork _unitOfWork;

    public Get(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/rooms/{roomId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("Rooms.Get", "", "Retrieves room details.")]
    [OpenApiTag("Rooms Endpoints")]
    public override async Task<ActionResult<ApiResult<RoomDetailsResult>>> HandleAsync(string roomId, CancellationToken cancellationToken = default)
    {
        var data = await _unitOfWork.Room.GetRoomFast(roomId);
        if (!data.Any())
        {
            return Ok(new ApiResult()
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "Room not found." }
            });
        }

        return Ok(new ApiResult<RoomDetailsResult>()
        {
            IsSuccess = true,
            Data = data.FirstOrDefault()
        });
    }
}