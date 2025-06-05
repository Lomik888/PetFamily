using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Application.AccauntManagment.Entities;

namespace PetFamily.Application.Providers;

public interface IJwtTokensProvider
{
    Result<string, ErrorList> CreateAccessJwtToken(User user);
}