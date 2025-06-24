namespace PetFemily.Accounts.Application.Dto;

public class RefreshSessionsDto
{
    public Guid Id { get; init; }
    public Guid Jti { get; init; }
    public Guid UserId { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime ExpireAt { get; init; }
}