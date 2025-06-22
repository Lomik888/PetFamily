using PetFamily.Core.Abstrations.Interfaces;

namespace PetFemily.Accounts.Application.Command.RefreshLogin;

public record RefreshLoginCommand(string Jwt, string RefreshToken) : ICommand;