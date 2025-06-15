using System.Data;

namespace PetFamily.Core.Abstrations;

public interface ISqlConnectionFactory
{
    IDbConnection Create();
}