using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.AccauntManagment.Command.Registration;

namespace PetFamily.API.Contracts.Requests.Accounts;

public record AccountRegistrationRequest(string Email, string Password) : IToCommand<AccountRegistrationCommand>
{
    public AccountRegistrationCommand ToCommand()
    {
        return new AccountRegistrationCommand(Email, Password);
    }
}