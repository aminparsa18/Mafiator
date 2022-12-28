using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using Mafiator.Common.Data.Enums;
using Mafiator.Repository;
using Mafiator.Service.Contracts.GameMembers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mafiator.Service.Services.GameMembers;

public class GameMemberService : IGameMemberService
{
    private readonly IUnitOfWork _unitOfWork;

    public GameMemberService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ApiResult<IEnumerable<GameMemberResult>>> GetByGame(string gameId)
    {
        var members = await _unitOfWork.GameMember.GetByGameFast(gameId);
        foreach (var gameMemberDto in members)
        {
            gameMemberDto.Image = string.Join(Data.Constants.BlobStorageEndpoint, gameMemberDto.Image);
        }
        return new ApiResult<IEnumerable<GameMemberResult>>
        {
            IsSuccess = true,
            Data = members
        };
    }

    public async Task<ApiResult<IEnumerable<PlayerRoleResult>>> GetMafiaPartners(string userId, string gameId)
    {
        var roles = await _unitOfWork.GameMember.GetRoleOfPlayer(userId, gameId);
        if (!roles.Any())
            return new ApiResult<IEnumerable<PlayerRoleResult>>
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "No such member found for this game" }
            };
        var role = roles.FirstOrDefault();
        if (role.Role != GameRole.GodFather && role.Role != GameRole.Mafia)
            return new ApiResult<IEnumerable<PlayerRoleResult>>
            {
                IsSuccess = false,
                StatusCode = ApiResultStatusCode.NotFound,
                Errors = new[] { "Reallyyy???!!! Only mafia players can use it" }
            };
        var partners = await _unitOfWork.GameMember.GetMafiaPartners(role.MemberId, gameId);

        return new ApiResult<IEnumerable<PlayerRoleResult>>
        {
            IsSuccess = true,
            Data = partners
        };
    }

    public async Task<ApiResult<PlayerRoleResult>> GetPlayerRole(string userId, string gameId)
    {
        var role = await _unitOfWork.GameMember.GetRoleOfPlayer(userId, gameId);
        if (role.Any())
            return new ApiResult<PlayerRoleResult>
            {
                IsSuccess = true,
                Data = role.FirstOrDefault()
            };
        return new ApiResult<PlayerRoleResult>
        {
            IsSuccess = false,
            StatusCode = ApiResultStatusCode.NotFound,
            Errors = new[] { "No such member found for this game" }
        };
    }

    public async Task<ApiResult<IEnumerable<WaitingPlayerResult>>> GetWaitingPlayersByGame(string gameId)
    {
        var members = await _unitOfWork.GameMember.GetWaitingPlayersByGame(gameId);
        foreach (var gameMemberDto in members)
        {
            gameMemberDto.Image = string.Join(Data.Constants.BlobStorageEndpoint, gameMemberDto.Image);
        }
        return new ApiResult<IEnumerable<WaitingPlayerResult>>
        {
            IsSuccess = true,
            Data = members
        };
    }
}