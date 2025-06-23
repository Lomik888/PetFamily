using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Enums;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Application.Command.Login;
using PetFemily.Accounts.Application.Providers;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Command.RefreshLogin;

public class RefreshLoginHandler : ICommandHandler<LoginResponseDto, ErrorList, RefreshLoginCommand>
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<RefreshLoginHandler> _logger;
    private readonly IValidator<RefreshLoginCommand> _validator;
    private readonly IJwtTokensProvider _tokensProvider;
    private readonly IAccountManager _accountManager;
    private readonly IUnitOfWork _unitOfWork;

    public RefreshLoginHandler(
        UserManager<User> userManager,
        ILogger<RefreshLoginHandler> logger,
        IValidator<RefreshLoginCommand> validator,
        IJwtTokensProvider tokensProvider,
        IAccountManager accountManager,
        [FromKeyedServices(UnitOfWorkTypes.Accounts)]
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _validator = validator;
        _tokensProvider = tokensProvider;
        _accountManager = accountManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<LoginResponseDto, ErrorList>> Handle(
        RefreshLoginCommand request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid validation request");
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var claimsResult = _tokensProvider.GetClaims(request.Jwt);
        if (claimsResult.IsFailure == true)
        {
            return ErrorList.Create(claimsResult.Error);
        }

        var claims = claimsResult.Value;

        var userIdClaim = claims.FirstOrDefault(x => x.Type == JwtClaimsTypesCustom.Sub);
        if (userIdClaim == null)
        {
            var error = ErrorsPreform.General.NotFound("Not found require claim");
            var errors = ErrorList.Create(error);
            return errors;
        }

        var userId = new Guid(userIdClaim.Value);
        var user = await _userManager.Users.SingleAsync(x => x.Id == userId, cancellationToken);

        var validJwt = _tokensProvider.ValidateJwt(user, request.Jwt);
        if (validJwt.IsFailure == true)
        {
            return validJwt.Error;
        }

        var jtiString = claims.FirstOrDefault(x => x.Type == JwtClaimsTypesCustom.Jti)!.Value;
        var jti = new Guid(jtiString);
        var refreshTokenOld = await _accountManager.GetRefreshSessionsAsync(userId, jti, cancellationToken);
        var tokensResult = _tokensProvider.CreateAccessJwtToken(user);
        if (tokensResult.IsFailure == true)
        {
            return validJwt.Error;
        }

        var newRefreshTokenResult = _tokensProvider.CreateRefreshToken(userId, new Guid(tokensResult.Value.jti));
        if (newRefreshTokenResult.IsFailure == true)
        {
            return validJwt.Error;
        }

        try
        {
            await _unitOfWork.BeginTransactionAsync(cancellationToken: cancellationToken);

            await _accountManager.DeleteRefreshSessionAsync(refreshTokenOld, cancellationToken);
            await _accountManager.AddRefreshSession(newRefreshTokenResult.Value, cancellationToken);

            await _unitOfWork.CommitAsync(cancellationToken);
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync(cancellationToken);
            _logger.LogError("Failed to create refresh session");
            return ErrorList
                .Create(ErrorsPreform.General.IternalServerError("Failed to create refresh session"));
        }

        var response = new LoginResponseDto(
            tokensResult.Value.jwt,
            newRefreshTokenResult.Value.Id.ToString(),
            newRefreshTokenResult.Value.ExpireAt);

        return response;
    }
}