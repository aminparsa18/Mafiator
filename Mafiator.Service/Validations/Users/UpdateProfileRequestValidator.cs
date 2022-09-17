using FluentValidation;
using Mafiator.Common.Data.Dtos.Users;

namespace Mafiator.Service.Validations.Users;

public class UpdateProfileRequestValidator : AbstractValidator<UpdateProfileRequest>
{
    public UpdateProfileRequestValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
    }
}