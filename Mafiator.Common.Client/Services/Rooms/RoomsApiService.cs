using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Rooms;

/// <inheritdoc/>
public class RoomsApiService : IRoomsApiService
{
    /// <inheritdoc/>
    public Task<HttpResponseMessage> AddRoom(RoomCreateRequest room)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri($"{UrlConstants.BaseUrl}rooms"), room);
    }

    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<RoomDetailsResult>>> GetMyRooms()
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<RoomDetailsResult>>>(
            new Uri($"{UrlConstants.BaseUrl}rooms"));
    }

    /// <inheritdoc/>
    public Task<ApiResult<RoomDetailsResult>> GetRoom(string roomId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<RoomDetailsResult>>(
            new Uri($"{UrlConstants.BaseUrl}rooms/{roomId}"));
    }

    /// <inheritdoc/>
    public Task<ApiResult<string>> IsRoomJoined(string roomId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<string>>(
            new Uri($"{UrlConstants.BaseUrl}rooms/isJoined/{roomId}"));
    }

    /// <inheritdoc/>
    public Task<HttpResponseMessage> JoinRoom(string code)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(
            new Uri($"{UrlConstants.BaseUrl}rooms/join/code/{code}"), code);
    }

    /// <inheritdoc/>
    public Task<HttpResponseMessage> LeaveRoom(LeaveRoomRequest request)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(
           new Uri($"{UrlConstants.BaseUrl}rooms/leave"), request);
    }
}