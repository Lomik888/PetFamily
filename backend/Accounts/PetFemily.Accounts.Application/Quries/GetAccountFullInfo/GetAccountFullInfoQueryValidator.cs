using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Quries.GetAccountFullInfo;

public class GetAccountFullInfoQueryValidator : AbstractValidator<GetAccountFullInfoQuery>
{
    public GetAccountFullInfoQueryValidator()
    {
        RuleFor(x => x.UserId).Must(x => x != Guid.Empty)
            .WithMessageCustom("UserId cannot be empty");
    }
}