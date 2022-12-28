using AutoMapper;
using Mafiator.Common.Data.Dtos.Games;
using Mafiator.Common.Data.Dtos.Rooms;
using Mafiator.Common.Data.Dtos.Users;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Data.Dtos.User;
using Mafiator.Entities.Identity;
using Mafiator.Entities.Models;
using System;

namespace Mafiator.Data;

/// <summary>
/// Custom Automapper profile used for DTO conversion.
/// </summary>
public sealed class AutoMapping : Profile
{
    public AutoMapping()
    {
        CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
        CreateMap<RegisterUserRequest, User>();
        CreateMap<RoomCreateRequest,Room>();
        CreateMap<GameCreateRequest, Game>();
        CreateMap<RefreshTokenDetails, RefreshToken>();
        CreateMap<GameEventRequest,GameEvent>();
    }
}