using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Users;

public interface IUserRegisterService
{
    Task<ApiResult> Register(RegisterUserRequest request);
}