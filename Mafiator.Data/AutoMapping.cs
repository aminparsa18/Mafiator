using AutoMapper;
using Mafiator.Data.Dtos.Game;
using Mafiator.Data.Dtos.GameEvent;
using Mafiator.Data.Dtos.Room;
using Mafiator.Data.Dtos.User;
using Mafiator.Entities;
using Mafiator.Entities.Identity;
using System;

namespace Mafiator.Data
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<string, Guid>().ConvertUsing(s => Guid.Parse(s));
            CreateMap<RegisterUserDto, User>();
            CreateMap<RoomCreateDto,Room>();
            CreateMap<GameCreateDto, Game>();
            CreateMap<RefreshTokenDto, RefreshToken>();
            CreateMap<GameEventDto,GameEvent>();
        }
    }
}