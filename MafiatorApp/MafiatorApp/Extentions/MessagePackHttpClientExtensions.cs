using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using MafiatorApp.Cache;
using MafiatorApp.Models.Api;
using MafiatorApp.Views;
using Polly;
using Polly.Retry;
using Xamarin.Forms;

namespace MafiatorApp.Extentions
{
     public static class MessagePackHttpClientExtensions
    {
        private static AsyncRetryPolicy<HttpResponseMessage> _refreshTokenPolicy;
        private static void CreateRefreshTokenPolicy()
        {
            _refreshTokenPolicy ??= Policy
                .HandleResult<HttpResponseMessage>(message => message.StatusCode == HttpStatusCode.Unauthorized)
                .RetryAsync(1, async (result, retryCount, context) =>
                {
                    var refreshTokenResult = await CheckToken();
                    if (!refreshTokenResult.IsSuccess)
                        throw new HttpRequestException(string.Join('-', refreshTokenResult.Errors));
                });

        }
        public static async Task<ApiResult> CheckToken()
        {
            if (!Barrel.Current.Exists("Token"))
            {
                Application.Current.MainPage = new NavigationPage(new LoginView());
                return new ApiResult()
                {
                    IsSuccess = false,
                    Errors = new[] { "Token does not exist" },
                    StatusCode = ApiResultStatusCode.NotFound
                };
            }

            var refreshTokenRequest = new RefreshTokenRequest()
            {
                Token = Barrel.Current.Get<string>("Token"),
                RefreshToken = Barrel.Current.Get<string>("RefreshToken")
            };
            var response = await RefreshToken(refreshTokenRequest);
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsMessagePackAsync<AuthResult>();
                if (!data.IsSuccess)
                    return new ApiResult() { IsSuccess = false, Errors = data.Errors };
                Barrel.Current.Add("Token", data.Token, TimeSpan.FromMinutes(6));
                Barrel.Current.Add("RefreshToken", data.RefreshToken, TimeSpan.FromDays(150));
                BaseHttpClient.Instance.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", data.Token);
                return new ApiResult() { IsSuccess = true };
            }
            else
            {
                var data = await response.Content.ReadAsMessagePackAsync<ApiResult>();
                return new ApiResult() { IsSuccess = false, Errors = data.Errors };
            }

        }
        public static Task<HttpResponseMessage> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {

            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri(Constants.BaseUrl + "api/User/RefreshToken"),
                refreshTokenRequest);
        }

        public static readonly string ContentTypeString = "application/x-msgpack";

        private static readonly MediaTypeWithQualityHeaderValue ContentTypeMediaTypeHeaderValue = new MediaTypeWithQualityHeaderValue(ContentTypeString);

        /// <summary>
        /// Adds default Acceot Header to given <see cref="HttpClient"/>.
        /// </summary>
        /// <param name="client">HttpClient to adjust.</param>
        public static void AddDefaultMessagePackAcceptHeader(this HttpClient client)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            if (!client.DefaultRequestHeaders.Accept.Contains(ContentTypeMediaTypeHeaderValue))
                client.DefaultRequestHeaders.Accept.Add(ContentTypeMediaTypeHeaderValue);
        }

        /// <summary>
        /// Calls given Uri and deserialize object from MessagePack.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="client">client to call</param>
        /// <param name="requestUri">Uri to call</param>
        /// <returns>Deserialized object.</returns>
        public static async Task<T> GetFromMessagePackAsync<T>(this HttpClient client, Uri requestUri)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            CreateRefreshTokenPolicy();
            var response = await _refreshTokenPolicy.ExecuteAsync(async context =>
            {
                using var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
                request.Headers.Add("Accept", ContentTypeString);
                request.Headers.Authorization =
                    new AuthenticationHeaderValue("Bearer", Barrel.Current.Get<string>("Token"));
                return await client.SendAsync(request, context).ConfigureAwait(false);
            }, CancellationToken.None).ConfigureAwait(false);
            return await response.Content.ReadAsMessagePackAsync<T>();
        }

      
        /// <summary>
        /// Post the given value using MessagePack formatter.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="client">client to use</param>
        /// <param name="requestUri">Uri to call</param>
        /// <param name="value">value</param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        public static async Task<HttpResponseMessage> PostAsMessagePackAsync<T>(this HttpClient client, Uri requestUri, T value)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));
            CreateRefreshTokenPolicy();
           return await _refreshTokenPolicy.ExecuteAsync(async context =>
            {
                using var content = new ObjectContent(typeof(T), value, MessagePackMediaTypeFormatter.DefaultInstance);
                return await client.PostAsync(requestUri, content, context);
            }, CancellationToken.None).ConfigureAwait(false);
        }

        /// <summary>
        /// Put the given value using MessagePack formatter.
        /// </summary>
        /// <typeparam name="T">Type of value</typeparam>
        /// <param name="client">client to use</param>
        /// <param name="requestUri">Uri to call</param>
        /// <param name="value">value</param>
        /// <returns><see cref="HttpResponseMessage"/></returns>
        public static async Task<HttpResponseMessage> PutAsMessagePackAsync<T>(this HttpClient client, Uri requestUri, T value)
        {
            if (client == null)
                throw new ArgumentNullException(nameof(client));

            using var content = new ObjectContent(typeof(T), value, MessagePackMediaTypeFormatter.DefaultInstance);
            return await client.PutAsync(requestUri, content).ConfigureAwait(false);
        }

       
    }
}