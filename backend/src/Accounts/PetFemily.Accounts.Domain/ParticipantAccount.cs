﻿namespace PetFemily.Accounts.Domain;

public class ParticipantAccount
{
    public const string RoleName = "Participant";
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public List<Guid> FavoritePetsIds { get; set; } = [];
}