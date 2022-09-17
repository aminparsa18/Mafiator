using FluentValidation;
using Mafiator.Common.Data.Dtos.Api.Auth;

namespace Mafiator.Service.Validations.Users;

public class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
    public RefreshTokenRequestValidator()
    {
        RuleFor(x => x.RefreshToken).NotEmpty();
        RuleFor(x => x.Token).NotEmpty();
    }
}