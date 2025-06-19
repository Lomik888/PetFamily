using System.Text.Json;
using PetFamily.Accounts.Infrastructure;
using PetFemily.Accounts.Domain;

namespace PetFamily.API;

public class SeedRolesPermissions
{
    private readonly AccountDbContext _accountDbContext;
    private readonly ILogger<SeedRolesPermissions> _logger;

    public SeedRolesPermissions(
        AccountDbContext accountDbContext,
        ILogger<SeedRolesPermissions> logger)
    {
        _accountDbContext = accountDbContext;
        _logger = logger;
    }

    public async Task Seed()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "RolesPermissions.json");
        var json = await File.ReadAllTextAsync(path);
        var rolesPermissions = JsonSerializer.Deserialize<RolesPermissions>(json) ??
                               throw new InvalidOperationException("RolesPermissions.json not found or null");

        var rolesWithPermissions = rolesPermissions.Roles.Select(x =>
        {
            var role = new Role()
            {
                Name = x.Key,
            };

            var permissions = x.Value.Select(x => new Permission()
            {
                Code = x
            });

            role.Permissions = permissions.ToList();
            return role;
        });

        await _accountDbContext.Roles.AddRangeAsync(rolesWithPermissions);
        await _accountDbContext.SaveChangesAsync();
    }
}