using Microsoft.EntityFrameworkCore;
using PetFemily.Accounts.Application;
using PetFemily.Accounts.Application.Dto;

namespace PetFamily.Accounts.Infrastructure;

public class AccountReadRepository : IAccountReadRepository
{
    private readonly ReadAccountDbContext _readDbContext;

    public AccountReadRepository(ReadAccountDbContext readDbContext)
    {
        _readDbContext = readDbContext;
    }

    public async Task<UserDto> GetFullInfoUserByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userDto = await _readDbContext.Users
            .Where(x => x.Id == userId)
            .Include(x => x.AdminAccount)
            .Include(x => x.VolunteerAccount)
            .Include(x => x.ParticipantAccount)
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .SingleAsync(cancellationToken);

        return userDto;
    }

    public async Task<bool> UserExistByIdAsync(Guid userId, CancellationToken cancellationToken)
    {
        var userExist = await _readDbContext.Users
            .AnyAsync(x => x.Id == userId, cancellationToken);

        return userExist;
    }
}