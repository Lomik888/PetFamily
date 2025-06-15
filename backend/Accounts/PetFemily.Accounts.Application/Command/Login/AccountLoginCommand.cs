using PetFamily.Core.Abstrations.Interfaces;

namespace PetFemily.Accounts.Application.Command.Login;

public record AccountLoginCommand(string Email, string Password) : ICommand;