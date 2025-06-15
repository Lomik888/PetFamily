using System.Data;
using Npgsql;
using PetFamily.Core.Abstrations;

namespace PetFamily.Volunteers.Infrastructure;

public class SqlConnectionFactory : ISqlConnectionFactory
{
    private readonly string _connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection Create()
    {
        return new NpgsqlConnection(_connectionString);
    }
}