using FluentValidation;
using PetFamily.Application.Extensions;

namespace PetFamily.Application.AccauntManagment.Command.Login;

public class AccountLoginCommandValidator : AbstractValidator<AccountLoginCommand>
{
    public AccountLoginCommandValidator()
    {
        RuleFor(x => x.Password)
            .MinimumLength(5).NotNull().NotEmpty().WithMessageCustom("Password or Email invalid");

        RuleFor(x => x.Email)
            .NotNull().NotEmpty().WithMessageCustom("Password or Email invalid");
    }
}