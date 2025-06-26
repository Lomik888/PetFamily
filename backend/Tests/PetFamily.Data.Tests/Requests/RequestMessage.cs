namespace PetFamily.Data.Tests.Requests;

public record RequestMessage(
    Guid UserId,
    string Text);