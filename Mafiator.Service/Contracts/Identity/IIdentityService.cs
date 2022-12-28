using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Identity;

public interface IIdentityService
{
    Task<IEnumerable<ValidateUserResult>> GetByUsername(string username);
}