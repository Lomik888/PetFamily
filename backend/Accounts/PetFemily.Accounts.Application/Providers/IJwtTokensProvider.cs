using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFemily.Accounts.Domain;

namespace PetFemily.Accounts.Application.Providers;

public interface IJwtTokensProvider
{
    Result<string, ErrorList> CreateAccessJwtToken(User user);
}