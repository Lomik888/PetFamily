using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Dto;

namespace PetFemily.Accounts.Application.Quries.GetAccountFullInfo;

public class GetAccountFullInfoQueryHandler : IQueryHandler<UserFullInfoDto, ErrorList, GetAccountFullInfoQuery>
{
    private readonly IValidator<GetAccountFullInfoQuery> _validator;
    private readonly IReadDbContext _readDbContext;

    public GetAccountFullInfoQueryHandler(
        IValidator<GetAccountFullInfoQuery> validator,
        IReadDbContext readDbContext)
    {
        _validator = validator;
        _readDbContext = readDbContext;
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

        var userFullInfo = await _readDbContext.Users
            .Where(x => x.Id == request.UserId)
            .Include(x => x.AdminAccount)
            .Include(x => x.VolunteerAccount)
            .Include(x => x.ParticipantAccount)
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .SingleOrDefaultAsync(cancellationToken);
        if (userFullInfo is null)
        {
            var error = ErrorsPreform.General.NotFound("User not found");
            return ErrorList.Create(error);
        }

        var permissionCodes = userFullInfo.Roles
            .SelectMany(x => x.Permissions.Select(x => x.Code));

        var userFullInfoDto = new UserFullInfoDto(
            userFullInfo.Id,
            userFullInfo.SocialNetworks.Items,
            userFullInfo.Photo?.FullPath,
            userFullInfo.FullName,
            userFullInfo!.Email!,
            userFullInfo.UserName!,
            userFullInfo.PhoneNumber,
            userFullInfo.Roles.Select(x => x.Name),
            permissionCodes,
            userFullInfo.AdminAccount?.Id,
            userFullInfo.VolunteerAccount?.Id,
            userFullInfo.ParticipantAccount?.Id,
            userFullInfo.ParticipantAccount?.FavoritePetsIds,
            userFullInfo.VolunteerAccount?.Certificates,
            userFullInfo.VolunteerAccount?.DetailsForHelps!.Items,
            userFullInfo.VolunteerAccount?.Experience.Value);

        return userFullInfoDto;
    }
}