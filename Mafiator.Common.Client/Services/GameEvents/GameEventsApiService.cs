using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.GameEvents;

/// <inheritdoc/>
public class GameEventsApiService : IGameEventsApiService
{
    /// <inheritdoc/>
    public Task<HttpResponseMessage> FireGameEvent(GameEventRequest gameEvent)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(
            new Uri($"{UrlConstants.BaseUrl}gameevents"), gameEvent);
    }

    /// <inheritdoc/>
    public Task<HttpResponseMessage> Cure(GameEventRequest gameEvent)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(
            new Uri($"{UrlConstants.BaseUrl}gameevents/cure"), gameEvent);
    }

    /// <inheritdoc/>
    public Task<HttpResponseMessage> Inquiry(GameEventRequest gameEvent)
    {
        return BaseHttpClient.Instance.PostAsMessagePackAsync(
            new Uri($"{UrlConstants.BaseUrl}api/GameEvent/Inquiry"), gameEvent);
    }

    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<GameEventResult>>> GetEventStatus(string gameId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<GameEventResult>>>(
            new Uri($"{UrlConstants.BaseUrl}gameevents/{gameId}"));
    }

    /// <inheritdoc/>
    public Task<ApiResult<IEnumerable<GameEventResult>>> GetNightResult(string gameId)
    {
        return BaseHttpClient.Instance.GetFromMemoryPackAsync<ApiResult<IEnumerable<GameEventResult>>>(
            new Uri($"{UrlConstants.BaseUrl}gameevents/night/{gameId}"));
    }
}