using Mafiator.Common.Data.Dtos.Api.Auth;
using Mafiator.Common.Data.Dtos.Users;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Users;

public interface IUserConfirmService
{
    Task<AuthResult> Confirm(ConfirmPhoneRequest request);
}