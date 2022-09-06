using Mafiator.Data.Dtos.Room;
using System;
using System.Collections.Generic;

namespace Mafiator.Repository.Contracts;

/// <summary>
/// Repository provides methods to retrieve/handle room data.
/// </summary>
public interface IRoomRepository : IBaseRepository<Room>
{
    /// <summary>
    /// Retrieves room details.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Room details.</returns>
    Task<IEnumerable<RoomDto>> GetRoomFast(string roomId);

    /// <summary>
    /// Retrieves list of rooms by pagination.
    /// </summary>
    /// <param name="skip">Skip.</param>
    /// <returns>List of rooms.</returns>
    Task<List<RoomDto>> GetDtoPage(int skip);

    /// <summary>
    /// Retrieves list of rooms by pagination.
    /// </summary>
    /// <param name="skip">Skip.</param>
    /// <returns>List of rooms.</returns>
    Task<IEnumerable<RoomDto>> GetDtoPageFast(int skip);

    /// <summary>
    /// Retrieves room details.
    /// </summary>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Room details.</returns>
    Task<RoomDto> GetRoom(string roomId);

    /// <summary>
    /// Retrieves all rooms by user.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <returns>List of user rooms.</returns>
    Task<List<RoomDto>> GetMyRooms(Guid userId);

    /// <summary>
    /// Retrieves all rooms by user.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <returns>List of user rooms.</returns>
    Task<IEnumerable<RoomDto>> GetMyRoomsFast(Guid userId);

    /// <summary>
    /// Retrievs room member if is joined in room.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Room member.</returns>
    Task<RoomMember> IsJoined(Guid userId, Guid roomId);

    /// <summary>
    /// Retrievs room member if is joined in room.
    /// </summary>
    /// <param name="userId">User key identifier.</param>
    /// <param name="roomId">Room key identifier.</param>
    /// <returns>Room member.</returns>
    Task<string> IsJoinedFast(string userId, string roomId);

    /// <summary>
    /// Retrievs room identifier by code.
    /// </summary>
    /// <param name="code">Room code.</param>
    /// <returns>Room key identifier.</returns>
    Task<string> GetByCode(string code);
}