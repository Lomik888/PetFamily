using System.Data;

namespace PetFamily.Application;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}