using PetFamily.Framework.Abstractions;
using PetFemily.Accounts.Application.Command.RefreshLogin;

namespace PetFamily.Accounts.Presentation.Requests;

public record AccountLoginRefreshRequest() : IToCommand<RefreshLoginCommand, string, string>
{
    public RefreshLoginCommand ToCommand(string jwt, string refreshToken)
    {
        return new RefreshLoginCommand(jwt, refreshToken);
    }
}