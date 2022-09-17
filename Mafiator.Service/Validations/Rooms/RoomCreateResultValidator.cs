using FluentValidation;
using Mafiator.Common.Data.Dtos.Rooms;
using System;

namespace Mafiator.Service.Validations.Rooms;

public class RoomCreateResultValidator : AbstractValidator<RoomCreateRequest>
{
    public RoomCreateResultValidator()
    {
        RuleFor(x=>x.Name).NotEmpty();
        RuleFor(x=>x.Country).NotEmpty();
        RuleFor(x => x.Users).ForEach(u => u.NotEqual(Guid.Empty));
    }
}