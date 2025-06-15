using PetFamily.Framework.Abstractions;
using PetFemily.Accounts.Application.Command.Login;

namespace PetFamily.Accounts.Presentation.Requests;

public record AccountLoginRequest(string Email, string Password) : IToCommand<AccountLoginCommand>
{
    public AccountLoginCommand ToCommand()
    {
        return new AccountLoginCommand(Email, Password);
    }
}