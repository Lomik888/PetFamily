using System.Text.Json;
using CSharpFunctionalExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PetFamily.Accounts.Infrastructure;
using PetFemily.Accounts.Domain;

namespace PetFamily.API;

public class SeedRolesPermissions
{
    private readonly AccountDbContext _accountDbContext;
    private readonly ILogger<SeedRolesPermissions> _logger;
    private readonly RoleManager<Role> _roleManager;

    public SeedRolesPermissions(
        AccountDbContext accountDbContext,
        ILogger<SeedRolesPermissions> logger,
        RoleManager<Role> roleManager)
    {
        _accountDbContext = accountDbContext;
        _logger = logger;
        _roleManager = roleManager;
    }

    public async Task Seed()
    {
        var path = Path.Combine(AppContext.BaseDirectory, "RolesPermissions.json");
        var json = await File.ReadAllTextAsync(path);
        var rolesPermissions = JsonSerializer.Deserialize<RolesPermissions>(json) ??
                               throw new InvalidOperationException("RolesPermissions.json not found or null");

        var roles = await _accountDbContext.Roles
            .Include(x => x.Permissions)
            .Select(x => new Role()
            {
                Name = x.Name,
                NormalizedName = x.NormalizedName,
                Permissions = x.Permissions.Select(x => new Permission()
                {
                    Code = x.Code
                }).ToList()
            })
            .ToListAsync();

        var rolesWithPermissions = rolesPermissions.Roles.Select(x =>
        {
            var role = new Role()
            {
                Name = x.Key,
                NormalizedName = x.Key.ToUpper()
            };

            var permissions = x.Value.Select(x => new Permission()
            {
                Code = x
            });

            role.Permissions = permissions.ToList();
            return role;
        }).ToList();

        var rolesHash = roles.OrderBy(x => x.Name).GetHashCode();
        var rolesWithPermissionsHash = rolesWithPermissions.OrderBy(x => x.Name).GetHashCode();

        if (rolesHash != rolesWithPermissionsHash && roles.Count != 0)
        {
            _logger.LogInformation("Roles permissions already exist.");
            return;
        }

        await _accountDbContext.Roles.AddRangeAsync(rolesWithPermissions);
        await _accountDbContext.SaveChangesAsync();
    }
}