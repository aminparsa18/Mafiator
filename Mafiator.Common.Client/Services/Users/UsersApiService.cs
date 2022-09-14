using Mafiator.Common.Client.Constants;
using Mafiator.Common.Client.Extensions;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mafiator.Common.Client.Services.Users
{
    /// <inheritdoc/>
    public class UsersApiService : IUsersApiService
    {
        /// <inheritdoc/>
        public Task<HttpResponseMessage> ConfirmPhoneNo(ConfirmPhoneRequest request)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri($"{UrlConstants.BaseUrl}users/confirm"), request);
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> Login(UserLoginRequest userLogin)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri($"{UrlConstants.BaseUrl}users/login"),
                userLogin);
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> RefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri($"{UrlConstants.BaseUrl}users/refresh"),
                refreshTokenRequest);
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> RegisterUser(RegisterUserRequest registerUserDto)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(new Uri($"{UrlConstants.BaseUrl}users/register"),
                registerUserDto);
        }

        /// <inheritdoc/>
        public Task<HttpResponseMessage> UpdateProfile(UpdateProfileRequest profile)
        {
            return BaseHttpClient.Instance.PostAsMessagePackAsync(
                new Uri($"{UrlConstants.BaseUrl}users/update"), profile);
        }

        /// <inheritdoc/>
        public Task<ApiResult<UserDetailsResult>> GetUser()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<UserDetailsResult>>(
                new Uri($"{UrlConstants.BaseUrl}users"));
        }

        /// <inheritdoc/>
        public Task<ApiResult<UserStatusResult>> GetUserStatus()
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<UserStatusResult>>(
                new Uri($"{UrlConstants.BaseUrl}users/status"));
        }

        /// <inheritdoc/>
        public Task<ApiResult<ValidateUserResult>> ValidateUser(string userCode)
        {
            return BaseHttpClient.Instance.GetFromMessagePackAsync<ApiResult<ValidateUserResult>>(
                new Uri($"{UrlConstants.BaseUrl}api/User/ValidateUser?username={userCode}"));
        }
    }
}