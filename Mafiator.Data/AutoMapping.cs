using System;
using AutoMapper;
using Mafiator.Data.Dtos;
using Mafiator.Entities;
using Mafiator.Entities.Identity;

namespace Mafiator.Data
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<string, Ulid>().ConvertUsing(s => Ulid.Parse(s));
            CreateMap<RegisterUserDto, User>();
            CreateMap<RoomCreateDto,Room>();
            CreateMap<GameCreateDto, Game>();
            CreateMap<RefreshTokenDto, RefreshToken>();
            CreateMap<GameEventDto,GameEvent>();
        }
    }
}