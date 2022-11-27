using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.ChatMessages;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.ChatMessages;

[Authorize]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ApiResult<IEnumerable<ChatMessageResult>>>
{
    private readonly IUnitOfWork _unitOfWork;

    public GetByRoom(IUnitOfWork unitOfWork)
    {
        this._unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/chatMessages/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByRoom), Tags = new[] { "Chat Messages Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<ChatMessageResult>>>> HandleAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        var chats = await _unitOfWork.ChatMessage.GetByRoom(roomId);
        chats.ForEach(c => c.Image = $"{Data.Constants.BlobStorageEndpoint}{c.Image}");
        return Ok(new ApiResult<List<ChatMessageResult>>()
        {
            IsSuccess = true,
            Data = chats
        });
    }
}