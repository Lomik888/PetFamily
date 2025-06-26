namespace PetFamily.Data.Tests.Requests;

public record RequestDiscussion(
    IEnumerable<Guid> UsersIds,
    Guid RelationId);