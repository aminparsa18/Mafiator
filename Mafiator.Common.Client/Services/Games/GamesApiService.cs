using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Games
{
    /// <inheritdoc/>
    public class GamesApiService : IGamesApiService
    {
        /// <inheritdoc/>
        public Task<HttpResponseMessage> AddGame(GameCreateRequest game)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri($"{UrlConstants.BaseUrl}games"), game);
        }

        /// <inheritdoc/>
        public Task<ApiResult<IEnumerable<AvailableGameResult>>> GetAvailableGames()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<AvailableGameResult>>>(
               new Uri($"{UrlConstants.BaseUrl}games/availables"));
        }

        /// <inheritdoc/>
        public Task<ApiResult<IEnumerable<RoomGameResult>>> GetGamesByRoom(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<IEnumerable<RoomGameResult>>>(
               new Uri($"{UrlConstants.BaseUrl}games/{roomId}"));
        }

        /// <inheritdoc/>
        public Task<ApiResult<AppointedGameResult>> GetAppointedGameDetails(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<AppointedGameResult>>(
                new Uri($"{UrlConstants.BaseUrl}games/appointed-details/{gameId}"));
        }

        /// <inheritdoc/>
        public Task<ApiResult<AppointedGameResult>> GetAppointedGame(string roomId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<AppointedGameResult>>(
                new Uri($"{UrlConstants.BaseUrl}games/appointed/{roomId}"));
        }

        /// <inheritdoc/>
        public Task<ApiResult<string>> IsGameJoined(string gameId)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<string>>(
                new Uri($"{UrlConstants.BaseUrl}games/isJoined/{gameId}"));
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> JoinGame(string gameId)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri($"{UrlConstants.BaseUrl}games/join/{gameId}"), null as string);
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> LeaveGame(string gameId)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri($"{UrlConstants.BaseUrl}games/leave/{gameId}"), null as string);
        }
    }
}