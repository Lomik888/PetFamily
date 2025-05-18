using Dapper;

namespace PetFamily.Application.Extensions;

public static class DynamicParametersExtensions
{
    public static DynamicParameters AddPagination(this DynamicParameters parameters, int page, int pageSize)
    {
        parameters.Add("@offset", pageSize * (page - 1));
        parameters.Add("@limit", pageSize);
        return parameters;
    }
}