using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Users;

public interface IUserLoginService
{
    Task<AuthResult> Login(UserLoginRequest request);
}