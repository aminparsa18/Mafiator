using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Games;

public interface IGameService
{
    Task<ApiResult<AppointedGameResult>> GetAppointed(Guid roomId);

    Task<ApiResult<AppointedGameResult>> GetAppointedDetails(Guid gameId);

    Task<ApiResult<IEnumerable<AvailableGameResult>>> GetAvailable();

    Task<ApiResult<IEnumerable<RoomGameResult>>> GetByRoom(string roomId);

    Task<ApiResult<string>> IsJoined(string userId, string gameId);
}