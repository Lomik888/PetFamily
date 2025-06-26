using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Providers;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Command.Login;

public class AccountLoginHandler : ICommandHandler<LoginResponseDto, ErrorList, AccountLoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<AccountLoginHandler> _logger;
    private readonly IValidator<AccountLoginCommand> _validator;
    private readonly IJwtTokensProvider _tokensProvider;
    private readonly IAccountManager _accountManager;

    public AccountLoginHandler(
        UserManager<User> userManager,
        ILogger<AccountLoginHandler> logger,
        IValidator<AccountLoginCommand> validator,
        IJwtTokensProvider tokensProvider,
        IAccountManager accountManager)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _tokensProvider = tokensProvider;
        _accountManager = accountManager;
    }

    public async Task<Result<LoginResponseDto, ErrorList>> Handle(
        AccountLoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user is null)
        {
            _logger.LogInformation("User not exists");
            var error = ErrorsPreform.General.NotFound(request.Email);
            return ErrorList.Create(error);
        }

        var result = await _userManager.CheckPasswordAsync(user, request.Password);
        if (result == false)
        {
            _logger.LogInformation("Password is wrong{0}", user.Email);
            var error = ErrorsPreform.General.Validation("email or password wrong", nameof(user));
            return ErrorList.Create(error);
        }

        var tokenResult = _tokensProvider.CreateAccessJwtToken(user);
        if (tokenResult.IsSuccess == false)
        {
            _logger.LogInformation("Cant Create jwt access token");
            var errors = tokenResult.Error;
            return ErrorList.Create(errors);
        }

        var jwtToken = tokenResult.Value.jwt;
        var jti = tokenResult.Value.jti;

        var refreshTokenResult = _tokensProvider.CreateRefreshToken(user.Id, new Guid(jti));
        if (refreshTokenResult.IsSuccess == false)
        {
            _logger.LogInformation("Cant Create refresh token");
            var errors = refreshTokenResult.Error;
            return ErrorList.Create(errors);
        }

        var refreshToken = refreshTokenResult.Value;

        await _accountManager.AddRefreshSession(refreshToken, cancellationToken);

        var response = new LoginResponseDto(jwtToken, refreshToken.Id.ToString(), refreshToken.ExpireAt);

        return response;
    }
}

public record LoginResponseDto(string Jwt, string RefreshToken, DateTime ExpireAt);