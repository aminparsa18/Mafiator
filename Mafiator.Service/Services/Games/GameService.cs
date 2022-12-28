using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Games;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Games;

public class GameService : IGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<AppointedGameResult>> GetAppointed(Guid roomId)
    {
        AppointedGameResult data = await _unitOfWork.Game.GetWaitingGameByRoom(roomId);
        data?.Members.ForEach(m => m.Image = Data.Constants.BlobStorageEndpoint + m.Image);
        return new ApiResult<AppointedGameResult>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public async Task<ApiResult<AppointedGameResult>> GetAppointedDetails(Guid gameId)
    {
        var data = await _unitOfWork.Game.GetWaitingGameInformations(gameId);
        data?.Members.ForEach(m => m.Image = Data.Constants.BlobStorageEndpoint + m.Image);
        return new ApiResult<AppointedGameResult>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public async Task<ApiResult<IEnumerable<AvailableGameResult>>> GetAvailable()
    {
        var data = await _unitOfWork.Game.GetAvailables();
        return new ApiResult<IEnumerable<AvailableGameResult>>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public async Task<ApiResult<IEnumerable<RoomGameResult>>> GetByRoom(string roomId)
    {
        var data = await _unitOfWork.Game.GetByRoom(roomId);
        return new ApiResult<IEnumerable<RoomGameResult>>
        {
            IsSuccess = true,
            Data = data
        };
    }

    public async Task<ApiResult<string>> IsJoined(string userId, string gameId)
    {
        var member = await _unitOfWork.Game.IsJoinedFast(userId, gameId);
        return new ApiResult<string>()
        {
            IsSuccess = true,
            Data = member
        };
    }
}