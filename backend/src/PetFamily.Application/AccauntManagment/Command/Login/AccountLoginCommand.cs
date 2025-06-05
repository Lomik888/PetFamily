using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.AccauntManagment.Command.Login;

public record AccountLoginCommand(string Email, string Password) : ICommand;