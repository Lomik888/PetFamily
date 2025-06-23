using System.Text.Json.Serialization;

namespace PetFemily.Accounts.Application.Dto;

public class ParticipantAccountDto
{
    public Guid Id { get; init; }
    public Guid UserId { get; init; }
    public List<Guid> FavoritePetsIds { get; init; } 
}