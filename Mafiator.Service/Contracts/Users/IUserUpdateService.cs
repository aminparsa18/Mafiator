using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Users;

public interface IUserUpdateService
{
    Task<ApiResult> Update(string userId, UpdateProfileRequest request);
}