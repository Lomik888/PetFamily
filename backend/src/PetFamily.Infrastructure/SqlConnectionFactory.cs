using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Application;
using PetFamily.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly IConfiguration _configuration;

    public SqlConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IDbConnection Create()
    {
        return new NpgsqlConnection(_configuration
            .GetRequiredSection(ApplicationDbContextOptions.CONNECTIONSTRING_SECTION_FOR_POSTGRESSQL)
            .GetValue<string>(ApplicationDbContextOptions.CONNECTIONSTRING_FOR_POSTGRESSQL));
    }
}