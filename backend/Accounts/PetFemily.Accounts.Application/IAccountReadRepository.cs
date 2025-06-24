using PetFemily.Accounts.Application.Dto;

namespace PetFemily.Accounts.Application;

public interface IAccountReadRepository
{
    Task<UserDto> GetFullInfoUserByIdAsync(
        Guid userId,
        CancellationToken cancellationToken);

    Task<bool> UserExistByIdAsync(Guid userId, CancellationToken cancellationToken);
}