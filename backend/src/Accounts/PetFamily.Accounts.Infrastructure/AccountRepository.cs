using Microsoft.EntityFrameworkCore;
using PetFemily.Accounts.Application;

namespace PetFamily.Accounts.Infrastructure;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _dbContext;

    public AccountRepository(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> GetRoleIdByNameAsync(string roleName)
    {
        return await _dbContext.Roles.Where(x => x.Name == roleName).Select(x => x.Id).FirstAsync();
    }
}