using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Identity
{
    public interface IIdentityService
    {
        Task<IEnumerable<ValidateUserResult>> GetByUsername(string username);
        Task<AuthResult> Login(UserLoginRequest userLogin);
        Task<ApiResult> Register(RegisterUserRequest registerUser);
        Task<AuthResult> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<AuthResult> ConfirmPhoneNumber(string phoneNo, string token);
        Task<ApiResult> UpdateProfile(string userId,string name,string image);
        Task<ApiResult<UserDetailsResult>> GetUser(string userId);
    }
}
