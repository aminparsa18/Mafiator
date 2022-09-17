using FluentValidation;
using Mafiator.Common.Data.Dtos.Votes;
using System;

namespace Mafiator.Service.Validations.Votes;

public class VoteCreateRequestValidator : AbstractValidator<VoteCreateRequest>
{
    public VoteCreateRequestValidator()
    {
        RuleFor(x => x.GameId).NotEqual(Guid.Empty);
        RuleFor(x => x.VoterId).NotEqual(Guid.Empty);
        RuleFor(x => x.Targets).ForEach(t=>t.NotEqual(Guid.Empty));
    }
}