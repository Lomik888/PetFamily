using System.Security.Claims;
using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Providers;

public interface IJwtTokensProvider
{
    Result<(string jwt, string jti), ErrorList> CreateAccessJwtToken(User user);
    Result<bool, ErrorList> ValidateJwt(User user, string userJwt);
    Result<RefreshSessions, ErrorList> CreateRefreshToken(Guid userId, Guid jti);
    Result<IReadOnlyList<Claim>, Error> GetClaims(string jwt);
}