using PetFamily.Framework.Abstractions;
using PetFemily.Accounts.Application.Command.Registration;

namespace PetFamily.Accounts.Presentation.Requests;

public record AccountRegistrationRequest(string Email, string Password) : IToCommand<AccountRegistrationCommand>
{
    public AccountRegistrationCommand ToCommand()
    {
        return new AccountRegistrationCommand(Email, Password);
    }
}