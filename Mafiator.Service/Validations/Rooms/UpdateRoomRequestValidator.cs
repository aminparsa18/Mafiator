using FluentValidation;
using Mafiator.Common.Data.Dtos.Rooms;
using System;

namespace Mafiator.Service.Validations.Rooms;

public class UpdateRoomRequestValidator : AbstractValidator<UpdateRoomNameRequest>
{
    public UpdateRoomRequestValidator()
    {
        RuleFor(x => x.RoomId).NotEqual(Guid.Empty);
        RuleFor(x => x.Name).NotEmpty();
    }
}