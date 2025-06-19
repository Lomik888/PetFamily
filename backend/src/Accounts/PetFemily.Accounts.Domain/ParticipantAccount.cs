namespace PetFemily.Accounts.Domain;

public class ParticipantAccount
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<Guid> FavoritePetsIds { get; set; } = [];
}