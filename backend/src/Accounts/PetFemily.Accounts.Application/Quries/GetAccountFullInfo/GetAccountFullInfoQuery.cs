using PetFamily.Core.Abstrations.Interfaces;

namespace PetFemily.Accounts.Application.Quries.GetAccountFullInfo;

public record GetAccountFullInfoQuery(Guid UserId) : IQuery;