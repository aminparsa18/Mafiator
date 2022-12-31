using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.GameMembers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.GameMembers;

/// <inheritdoc/>
public class GameMemberApiService : IGameMemberApiService
{
    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<PlayerRoleResult>>> GetMafiaPartners(string gameId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<PlayerRoleResult>>>(
            new Uri($"{UrlConstants.BaseUrl}gamemembers/mafia-partners/{gameId}"));
    }

    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<GameMemberResult>>> GetMembersOfGame(string gameId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<GameMemberResult>>>(
            new Uri($"{UrlConstants.BaseUrl}gamemembers/{gameId}"));
    }

    /// <inheritdoc/>
    public Task<ApiResult<PlayerRoleResult>> GetPlayerRole(string gameId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<PlayerRoleResult>>(
          new Uri($"{UrlConstants.BaseUrl}gamemembers/role/{gameId}"));
    }

    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<WaitingPlayerResult>>> GetWaitingPlayersByGame(string gameId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<WaitingPlayerResult>>>(
           new Uri($"{UrlConstants.BaseUrl}gamemembers/waitings/{gameId}"));
    }
}