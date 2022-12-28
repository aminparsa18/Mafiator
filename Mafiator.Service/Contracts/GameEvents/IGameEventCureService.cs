using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.GameEvents;

public interface IGameEventCureService
{
    Task<ApiResult> Cure(string userId, GameEventRequest request);
}