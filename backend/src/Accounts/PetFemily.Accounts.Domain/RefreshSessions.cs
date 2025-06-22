namespace PetFemily.Accounts.Domain;

public class RefreshSessions
{
    public Guid Id { get; set; }
    public Guid Jti { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpireAt { get; set; }
}