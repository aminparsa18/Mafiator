using FluentValidation;
using Mafiator.Common.Data.Dtos.Rooms;
using System;

namespace Mafiator.Service.Validations.Rooms;

public class LeaveRoomRequestValidator : AbstractValidator<LeaveRoomRequest>
{
    public LeaveRoomRequestValidator()
    {
        RuleFor(x=>x.RoomId).NotEqual(Guid.Empty);
    }
}