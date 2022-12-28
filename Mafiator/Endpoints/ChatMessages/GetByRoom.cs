using Mafiator.Common.Data.Dtos.ChatMessages;
using Mafiator.Service.Contracts.ChatMessages;
using System;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.ChatMessages;

[Authorize]
public class GetByRoom : EndpointBaseAsync
    .WithRequest<Guid>
    .WithActionResult<ApiResult<IEnumerable<ChatMessageResult>>>
{
    private readonly IChatMessageService _chatMessageService;

    public GetByRoom(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    [ApiVersion("1.0")]
    [HttpGet("api/v{version:apiVersion}/chatMessages/{roomId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [SwaggerOperation(OperationId = nameof(GetByRoom), Tags = new[] { "Chat Messages Endpoints" })]
    public override async Task<ActionResult<ApiResult<IEnumerable<ChatMessageResult>>>> HandleAsync(Guid roomId, CancellationToken cancellationToken = default)
    {
        return Ok(new ApiResult<List<ChatMessageResult>>()
        {
            IsSuccess = true,
            Data = await _chatMessageService.GetByRoom(roomId)
        });
    }
}