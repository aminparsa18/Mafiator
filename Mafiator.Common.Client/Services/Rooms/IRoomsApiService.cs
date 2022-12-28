using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Rooms;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Rooms;

/// <summary>
/// API service provides methods to retrieve/handle rooms.
/// </summary>
public interface IRoomsApiService
{
    /// <summary>
    /// Get room details.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Room details result.</returns>
    Task<ApiResult<RoomDetailsResult>> GetRoom(string roomId);

    /// <summary>
    /// Get all rooms I am member of.
    /// </summary>
    /// <returns>List of room details api result.</returns>
    Task<ApiResult<IEnumerable<RoomDetailsResult>>> GetMyRooms();

    /// <summary>
    /// Creates a new room.
    /// </summary>
    /// <param name="room">Room key identifier.</param>
    /// <returns>HTTP response message.</returns>
    Task<HttpResponseMessage> AddRoom(RoomCreateRequest room);

    /// <summary>
    /// Retrieves room member identifier if is joined in room.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Room member identifier api result.</returns>
    Task<ApiResult<string>> IsRoomJoined(string roomId);

    /// <summary>
    /// Joins a room by code.
    /// </summary>
    /// <param name="code">Room code.</param>
    /// <returns>HTTP response message.</returns>
    Task<HttpResponseMessage> JoinRoom(string code);

    /// <summary>
    /// Leaves a room.
    /// </summary>
    /// <param name="request">Leave room request.</param>
    /// <returns>HTTP response message.</returns>
    Task<HttpResponseMessage> LeaveRoom(LeaveRoomRequest request);
}