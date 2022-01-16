using Mafiator.Common.Api;
using Mafiator.Data.Dtos.User;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Identity
{
    public interface IIdentityService
    {
        Task<IEnumerable<ValidateUserDto>> GetByUsername(string username);
        Task<AuthResult> Login(UserLoginDto userLogin);
        Task<ApiResult> Register(RegisterUserDto registerUser);
        Task<AuthResult> RefreshToken(RefreshTokenRequest refreshTokenRequest);
        Task<AuthResult> ConfirmPhoneNumber(string phoneNo, string token);
        Task<ApiResult> UpdateProfile(string userId,string name,string image);
        Task<ApiResult<UserDto>> GetUser(string userId);
    }
}
