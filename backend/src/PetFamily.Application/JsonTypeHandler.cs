using System.Data;
using System.Text.Json;
using Dapper;

namespace PetFamily.Application.VolunteerUseCases.Queries.GetPet;

public class JsonTypeHandler<T> : SqlMapper.TypeHandler<T>
{
    public override void SetValue(IDbDataParameter parameter, T? value)
    {
        parameter.Value = JsonSerializer.Serialize(value);
    }

    public override T? Parse(object value)
    {
        return JsonSerializer.Deserialize<T>(value.ToString() ?? string.Empty);
    }
}