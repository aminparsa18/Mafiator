using FluentValidation;
using Mafiator.Data.Dtos.GameEvent;
using System;

namespace Mafiator.Service.Validations.GameEvents;

public class GameEventRequestValidator : AbstractValidator<GameEventRequest>
{
    public GameEventRequestValidator()
    {
        RuleFor(x => x.GameId).NotEqual(Guid.Empty);
        RuleFor(x => x.MemberId).NotEqual(Guid.Empty);
    }
}