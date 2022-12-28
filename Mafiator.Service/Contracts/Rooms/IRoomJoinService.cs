using Mafiator.Common.Data.Dtos.Api;
using System;
using System.Threading.Tasks;

namespace Mafiator.Service.Contracts.Rooms;

public interface IRoomJoinService
{
    Task<ApiResult<string>> JoinByCode(string userId, string code);

    Task<ApiResult> JoinById(string userId, Guid id);
}