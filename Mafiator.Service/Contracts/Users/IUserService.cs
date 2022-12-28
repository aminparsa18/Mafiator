using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Users;

public interface IUserService
{
    Task<ApiResult<UserDetailsResult>> GetDetails(string userId);

    Task<ApiResult<UserStatusResult>> GetStatus(string userId);
}