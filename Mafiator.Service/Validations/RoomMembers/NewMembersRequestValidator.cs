using FluentValidation;
using Mafiator.Common.Data.Dtos.RoomMembers;
using System;

namespace Mafiator.Service.Validations.RoomMembers;

public class NewMembersRequestValidator : AbstractValidator<NewMembersRequest>
{
    public NewMembersRequestValidator()
    {
        RuleFor(x => x.RoomId).NotEqual(Guid.Empty);
        RuleFor(x => x.Users).ForEach(u => u.NotEqual(Guid.Empty));
    }
}