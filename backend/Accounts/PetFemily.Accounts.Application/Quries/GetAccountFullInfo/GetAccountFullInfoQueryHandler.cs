using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Dto;

namespace PetFemily.Accounts.Application.Quries.GetAccountFullInfo;

public class GetAccountFullInfoQueryHandler : IQueryHandler<UserFullInfoDto, ErrorList, GetAccountFullInfoQuery>
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

    public async Task<Result<UserFullInfoDto, ErrorList>> Handle(
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

        var permissionCodes = userFullInfo.Roles
            .SelectMany(x => x.Permissions.Select(x => x.Code));

        var userFullInfoDto = new UserFullInfoDto(
            userFullInfo.Id,
            userFullInfo.SocialNetworks,
            userFullInfo.Photo,
            userFullInfo.FullName,
            userFullInfo.Email,
            userFullInfo.UserName!,
            userFullInfo.PhoneNumber,
            userFullInfo.Roles.Select(x => x.Name),
            permissionCodes,
            userFullInfo.AdminAccount?.Id,
            userFullInfo.VolunteerAccount?.Id,
            userFullInfo.ParticipantAccount?.Id,
            userFullInfo.ParticipantAccount?.FavoritePetsIds,
            userFullInfo.VolunteerAccount?.Certificates,
            userFullInfo.VolunteerAccount?.DetailsForHelps!,
            userFullInfo.VolunteerAccount?.Experience);

        return userFullInfoDto;
    }
}