using FluentValidation;
using PetFamily.Core.Extensions;

namespace PetFemily.Accounts.Application.Command.Registration;

public class AccountRegistrationCommandValidator : AbstractValidator<AccountRegistrationCommand>
{
    public AccountRegistrationCommandValidator()
    {
        RuleFor(x => x.Password)
            .MinimumLength(5).NotNull().NotEmpty().WithMessageCustom("Password or Email invalid");
        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessageCustom("Password or Email invalid");
    }
}