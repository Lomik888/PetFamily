using System.Text.Json;
using DotNetEnv;
using Microsoft.AspNetCore.Identity;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Enums;
using PetFemily.Accounts.Domain;

namespace PetFamily.API;

public class SeedAccounts
{
    private readonly UserManager<User> _userManager;
    private readonly ILogger<SeedAccounts> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public SeedAccounts(
        UserManager<User> userManager,
        ILogger<SeedAccounts> logger,
        [FromKeyedServices(UnitOfWorkTypes.Accounts)]
        IUnitOfWork unitOfWork)
    {
        _userManager = userManager;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task Seed()
    {
        var userName = Env.GetString("USER_NAME");
        var email = Env.GetString("ADMIN_EMAIL");
        var password = Env.GetString("ADMIN_PASSWORD");

        var adminExists = await _userManager.FindByEmailAsync(email);
        if (adminExists != null)
        {
            _logger.LogInformation("User Admin already exists.");
            return;
        }

        var userAdmin = User.CreateAdmin(userName, email, userName);

        try
        {
            await _unitOfWork.BeginTransactionAsync();

            var userAdminResult = await _userManager.CreateAsync(userAdmin, password);
            if (userAdminResult.Succeeded == false)
            {
                var json = JsonSerializer.Serialize(userAdminResult.Errors);
                _logger.LogError(json);
                throw new ApplicationException();
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(userAdmin, AdminAccount.RoleName.ToUpper());
            if (addToRoleResult.Succeeded == false)
            {
                var json = JsonSerializer.Serialize(addToRoleResult.Errors);
                _logger.LogError(json);
                throw new ApplicationException();
            }

            await _unitOfWork.CommitAsync();
        }
        catch (Exception e)
        {
            await _unitOfWork.RollbackAsync();
        }
    }
}