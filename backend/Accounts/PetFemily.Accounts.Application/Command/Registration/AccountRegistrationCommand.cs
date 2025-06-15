using PetFamily.Core.Abstrations.Interfaces;

namespace PetFemily.Accounts.Application.Command.Registration;

public record AccountRegistrationCommand(string Email, string Password) : ICommand;