using PetFamily.API.Contracts.Requests.Interfaces;
using PetFamily.Application.AccauntManagment.Command.Login;

namespace PetFamily.API.Contracts.Requests.Accounts;

public record AccountLoginRequest(string Email, string Password) : IToCommand<AccountLoginCommand>
{
    public AccountLoginCommand ToCommand()
    {
        return new AccountLoginCommand(Email, Password);
    }
}