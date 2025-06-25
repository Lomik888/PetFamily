using FluentValidation;
using PetFamily.Core.Extensions;

namespace PetFemily.Accounts.Application.Quries.GetAccountFullInfo;

public class GetAccountFullInfoQueryValidator : AbstractValidator<GetAccountFullInfoQuery>
{
    public GetAccountFullInfoQueryValidator()
    {
        RuleFor(x => x.UserId).Must(x => x != Guid.Empty)
            .WithMessageCustom("UserId cannot be empty");
    }
}