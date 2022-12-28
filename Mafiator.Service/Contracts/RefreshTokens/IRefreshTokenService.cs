using Mafiator.Common.Data.Dtos.Api.Auth;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.RefreshTokens;

public interface IRefreshTokenService
{
    Task<AuthResult> Refresh(RefreshTokenRequest request);
}