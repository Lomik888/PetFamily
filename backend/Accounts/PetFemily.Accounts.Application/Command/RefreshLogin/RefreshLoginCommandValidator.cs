using FluentValidation;
using PetFamily.Core.Extensions;

namespace PetFemily.Accounts.Application.Command.RefreshLogin;

public class RefreshLoginCommandValidator : AbstractValidator<RefreshLoginCommand>
{
    public RefreshLoginCommandValidator()
    {
        RuleFor(x => x.RefreshToken).Must(x => string.IsNullOrWhiteSpace(x) == false)
            .WithMessageCustom("Refresh token is required");
        RuleFor(x => x.Jwt).Must(x => string.IsNullOrWhiteSpace(x) == false)
            .WithMessageCustom("Refresh token is required");
    }
}