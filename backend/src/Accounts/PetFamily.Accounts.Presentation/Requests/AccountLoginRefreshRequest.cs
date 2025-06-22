using PetFamily.Framework.Abstractions;
using PetFemily.Accounts.Application.Command.RefreshLogin;

namespace PetFamily.Accounts.Presentation.Requests;

public record AccountLoginRefreshRequest(string RefreshToken) : IToCommand<RefreshLoginCommand, string>
{
    public RefreshLoginCommand ToCommand(string jwt)
    {
        return new RefreshLoginCommand(jwt, RefreshToken);
    }
}