namespace PetFamily.Infrastructure.Authorization.Options;

public sealed class JwtOptions
{
    public string Jwt = nameof(Jwt);
    public string Audience { get; set; }
    public string Issuer { get; set; }
    public string SecurityKey { get; set; }
    public int LifeTimeMinutes { get; set; }
}