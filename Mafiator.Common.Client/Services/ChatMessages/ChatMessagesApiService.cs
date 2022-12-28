using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.ChatMessages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.ChatMessages;

/// <inheritdoc/>
public class ChatMessagesApiService : IChatMessagesApiService
{
    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<ChatMessageResult>>> GetChatByRoom(string roomId)
    {
        return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<ChatMessageResult>>>(
           new Uri($"{UrlConstants.BaseUrl}chatMessages/{roomId}"));
    }
}