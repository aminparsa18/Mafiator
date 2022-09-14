using Ardalis.ApiEndpoints;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.ChatMessages;
using Mafiator.Data;
using Mafiator.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSwag.Annotations;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Mafiator.Api.Endpoints.ChatMessages;

[Produces("application/x-msgpack")]
[Consumes("application/x-msgpack")]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ApiResult<IEnumerable<ChatMessageResult>>>
{
    private readonly IUnitOfWork unitOfWork;

    public GetByRoom(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/chatMessages/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [OpenApiOperation("ChatMessages.GetByRoom", "", "Retrieves all chat messages in a room.")]
    [OpenApiTag("Chat Messages Endpoints")]
    public override async Task<ActionResult<ApiResult<IEnumerable<ChatMessageResult>>>> HandleAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        var chats = await unitOfWork.ChatMessage.GetByRoom(roomId);
        chats.ForEach(c => c.Image = $"{Constants.BlobStorageEndpoint}{c.Image}");
        return Ok(new ApiResult<List<ChatMessageResult>>()
        {
            IsSuccess = true,
            Data = chats
        });
    }
}