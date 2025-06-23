using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Dto;

namespace PetFemily.Accounts.Application.Quries.GetAccountFullInfo;

public class GetAccountFullInfoQueryHandler : IQueryHandler<UserDto, ErrorList, GetAccountFullInfoQuery>
{
    private readonly IValidator<GetAccountFullInfoQuery> _validator;
    private readonly IAccountManager _accountManager;

    public GetAccountFullInfoQueryHandler(
        IValidator<GetAccountFullInfoQuery> validator,
        IAccountManager accountManager)
    {
        _validator = validator;
        _accountManager = accountManager;
    }

    public async Task<Result<UserDto, ErrorList>> Handle(
        GetAccountFullInfoQuery request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var userExist = await _accountManager.UserExistByIdAsync(request.UserId, cancellationToken);
        if (userExist == false)
        {
            var error = ErrorsPreform.General.NotFound("User not found");
            return ErrorList.Create(error);
        }

        var userFullInfo = await _accountManager.GetFullInfoUserByIdAsync(request.UserId, cancellationToken);

        return userFullInfo;
    }
}