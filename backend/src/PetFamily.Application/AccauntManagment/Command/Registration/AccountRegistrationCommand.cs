using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.AccauntManagment.Command.Registration;

public record AccountRegistrationCommand(string Email, string Password) : ICommand;