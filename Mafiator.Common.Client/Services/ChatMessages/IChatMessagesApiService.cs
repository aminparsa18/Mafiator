using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.ChatMessages;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.ChatMessages
{
    /// <summary>
    /// API service provides methods to retrieve/handle chat messages.
    /// </summary>
    public interface IChatMessagesApiService
    {
        /// <summary>
        /// Retrieves all chat messages in a room.
        /// </summary>
        /// <param name="roomId">Room key identifier</param>
        /// <returns>List of chat messages api result.</returns>
        Task<ApiResult<IEnumerable<ChatMessageResult>>> GetChatByRoom(string roomId);
    }
}