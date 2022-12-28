using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Common.Data.Dtos.Games;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Games;

public interface IGameCreateService
{
    Task<ApiResult<GameCreateResult>> Create(GameCreateRequest request);
}