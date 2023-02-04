using Mafiator.Common.Data.Dtos.ChatMessages;
using Mafiator.Service.Contracts.ChatMessages;
using System;
using System.Collections.Generic;

namespace Mafiator.Api.Endpoints.ChatMessages;

public class GetByRoom : EndpointWithoutRequest<ApiResult<IEnumerable<ChatMessageResult>>>
{
    private readonly IChatMessageService _chatMessageService;

    public GetByRoom(IChatMessageService chatMessageService)
    {
        _chatMessageService = chatMessageService;
    }

    public override void Configure()
    {
        Get("api/v1/chatMessages/{roomId}");
        Summary(s =>
        {
            s.Summary = "Get messages by room";
            s.Description = "Retrieves all chat messages in a room";
        });
        Description(d => d.Produces(200).WithTags(EndpointsTags.ChatMessages));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        string roomId = Route<string>("roomId");
        await SendMemoryPackAsync(new ApiResult<IEnumerable<ChatMessageResult>>()
        {
            IsSuccess = true,
            Data = await _chatMessageService.GetByRoom(Guid.Parse(roomId))
        }, cancellation: ct);
    }
}