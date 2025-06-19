using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Extensions;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Command.Registration;

public class AccountRegistrationHandler : ICommandHandler<ErrorList, AccountRegistrationCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AccountRegistrationHandler> _logger;
    private readonly IValidator<AccountRegistrationCommand> _validator;

    public AccountRegistrationHandler(
        UserManager<User> userManager,
        ILogger<AccountRegistrationHandler> logger,
        IValidator<AccountRegistrationCommand> validator)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
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

        var user = new User()
        {
            UserName = request.Email,
           // Email = request.Email
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (result.Succeeded == false)
        {
            _logger.LogInformation("Can't create user, something wrong");
            var errors = result.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        _logger.LogInformation("User created a new account with password.");
        return UnitResult.Success<ErrorList>();
    }
}