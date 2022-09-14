using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.RoomMembers
{
    /// <summary>
    /// API service provides methods to retrieve/handle room members.
    /// </summary>
    public interface IRoomMembersApiService
    {
        /// <summary>
        /// Add new members to room.
        /// </summary>
        /// <param name="request">New members add request.</param>
        /// <returns>Http response message.</returns>
        Task<HttpResponseMessage> AddMember(NewMembersRequest request);

        /// <summary>
        /// Retrieves members in a room.
        /// </summary>
        /// <param name="roomId">Room key identifier.</param>
        /// <returns>List of room members api result.</returns>
        Task<ApiResult<IEnumerable<RoomMemberResult>>> GetMembersByRoom(Guid roomId);
    }
}