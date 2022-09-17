using FluentValidation;
using Mafiator.Common.Data.Dtos.Games;
using System;

namespace Mafiator.Service.Validations.Games;

public class GameCreateRequestValidator : AbstractValidator<GameCreateRequest>
{
    public GameCreateRequestValidator()
    {
        RuleFor(x => x.RoomId).NotEqual(Guid.Empty);
        RuleFor(x => x.Roles).NotEmpty();
        RuleFor(x=>x.StartDate).NotEqual(DateTime.MinValue);
    }
}