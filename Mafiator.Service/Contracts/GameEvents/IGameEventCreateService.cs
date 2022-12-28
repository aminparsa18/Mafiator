using FluentValidation.Results;
using Mafiator.Common.Data.Dtos.Api;
using Mafiator.Data.Dtos.GameEvent;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.GameEvents;

public interface IGameEventCreateService
{
    Task<ApiResult> Create(GameEventRequest request);
}