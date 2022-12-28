using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.RoomMembers;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.RoomMembers;

/// <inheritdoc/>
public class RoomMembersApiService : IRoomMembersApiService
{
    /// <inheritdoc/>
    public Task<HttpResponseMessage> AddMember(NewMembersRequest request)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(
           new Uri($"{UrlConstants.BaseUrl}roommembers"), request);
    }

    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<RoomMemberResult>>> GetMembersByRoom(Guid roomId)
    {
        return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomMemberResult>>>(
            new Uri($"{UrlConstants.BaseUrl}roommembers/{roomId}"));
    }
}