namespace PetFemily.Accounts.Application;

public interface IAccountRepository
{
    Task<Guid> GetRoleIdByNameAsync(string roleName);
}