using System.Text.Json;
using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Enums;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Command.Registration;

public class AccountRegistrationHandler : ICommandHandler<ErrorList, AccountRegistrationCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AccountRegistrationHandler> _logger;
    private readonly IValidator<AccountRegistrationCommand> _validator;

    public AccountRegistrationHandler(
        UserManager<User> userManager,
        ILogger<AccountRegistrationHandler> logger,
        IValidator<AccountRegistrationCommand> validator,
        [FromKeyedServices(UnitOfWorkTypes.Accounts)]
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _unitOfWork = unitOfWork;
    }

    public async Task<UnitResult<ErrorList>> Handle(
        AccountRegistrationCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var userExists = await _userManager.FindByEmailAsync(request.Email);
        if (userExists is not null)
        {
            _logger.LogInformation("User already exists");
            var error = ErrorsPreform.General.Validation("User already exists", request.Email);
            return ErrorList.Create(error);
        }

        var userParticipant = User.CreateParticipantAccount(
            request.Email,
            request.Email,
            request.Email);
        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            var userParticipantResult = await _userManager.CreateAsync(userParticipant, request.Password);
            if (userParticipantResult.Succeeded == false)
            {
                var json = JsonSerializer.Serialize(userParticipantResult.Errors);
                _logger.LogError(json);
                throw new ApplicationException();
            }

            var addToRoleResult =
                await _userManager.AddToRoleAsync(userParticipant, ParticipantAccount.RoleName.ToUpper());
            if (addToRoleResult.Succeeded == false)
            {
                var json = JsonSerializer.Serialize(addToRoleResult.Errors);
                _logger.LogError(json);
                throw new ApplicationException();
            }

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
        }

        _logger.LogInformation("User created a new account with password.");
        return UnitResult.Success<ErrorList>();
    }
}