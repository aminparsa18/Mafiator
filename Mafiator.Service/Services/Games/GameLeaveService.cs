using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Repository;
using Mafiator.Service.Contracts.Games;
using Mafiator.Service.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.Games;

public class GameLeaveService : IGameLeaveService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubContext<GameHub> _gameHub;

    public GameLeaveService(IUnitOfWork unitOfWork, IHubContext<GameHub> gameHub)
    {
        _unitOfWork = unitOfWork;
        _gameHub = gameHub;
    }

    public async Task<ApiResult> Leave(string userId, string gameId)
    {
        var member = await _unitOfWork.GameMember.GetUser(userId, gameId);
        if (!member.Any())
            return new ApiResult<string>
            {
                IsSuccess = false,
                Errors = new[] { "You are not member of this game" },
                StatusCode = ApiResultStatusCode.NotFound
            };
        await _unitOfWork.GameMember.Leave(userId);
        await _gameHub.Clients.Group(gameId).SendAsync("Leave", userId.ToString());
        return new ApiResult<string>
        {
            IsSuccess = true,
        };
    }
}