using FluentValidation;
using Mafiator.Common.Data.Dtos.Users;

namespace Mafiator.Service.Validations.Users;
public class ConfirmPhoneRequestValidator : AbstractValidator<ConfirmPhoneRequest>
{
    public ConfirmPhoneRequestValidator()
    {
        RuleFor(x => x.PhoneNo).NotEmpty();
        RuleFor(x => x.Token).NotEmpty().Length(6);
    }
}