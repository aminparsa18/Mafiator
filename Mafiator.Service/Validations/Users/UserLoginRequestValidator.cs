using FluentValidation;
using Mafiator.Common.Data.Dtos.Users;

namespace Mafiator.Service.Validations.Users;

public class UserLoginRequestValidator : AbstractValidator<UserLoginRequest>
{
    public UserLoginRequestValidator()
    {
        RuleFor(x=> x.Username).NotEmpty();
        RuleFor(x=> x.Password).NotEmpty().MinimumLength(6);
    }
}