using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace PetFamily.Infrastructure.DbContext.PostgresSQL;

public sealed record ApplicationDbContextOptions : IOptions<ApplicationDbContextOptions>
{
    public const string CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL = "Postgres_SQL";

    [ConfigurationKeyName("Postgres_SQL")] 
    public string? ConnectionString { get; }

    public ApplicationDbContextOptions Value { get; }
}