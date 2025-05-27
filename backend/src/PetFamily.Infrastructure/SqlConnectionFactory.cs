using System.Data;
using Microsoft.Extensions.Configuration;
using Npgsql;
using PetFamily.Application;
using PetFamily.Infrastructure.DbContext.PostgresSQL;

namespace PetFamily.Infrastructure;

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