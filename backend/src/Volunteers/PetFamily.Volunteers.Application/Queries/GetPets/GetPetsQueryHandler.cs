using System.Text;
using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Core;
using PetFamily.Core.Abstrations;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Dtos.PetDtos;

namespace PetFamily.Volunteers.Application.Queries.GetPets;

public class GetPetsQueryHandler : IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetPetsQueryHandler> _logger;
    private readonly IValidator<GetPetsQuery> _validator;

    public GetPetsQueryHandler(
        IValidator<GetPetsQuery> validator,
        ILogger<GetPetsQueryHandler> logger,
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _validator = validator;
        _logger = logger;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<GetObjectsWithPaginationResponse<PetDto>, ErrorList>> Handle(
        GetPetsQuery request,
        CancellationToken cancellationToken = default)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var result = await GetPetDto(request, cancellationToken);
        if (result.IsSuccess == false)
        {
            var errors = validationResult.Errors.ToErrors();
            return ErrorList.Create(errors);
        }

        var getObjectsWithPaginationResponse = new GetObjectsWithPaginationResponse<PetDto>()
        {
            Data = result.Value.PetsDtos,
            Count = result.Value.PetCount,
            Page = request.Page,
            PageSize = request.PageSize
        };

        return getObjectsWithPaginationResponse;
    }

    private async Task<Result<(long PetCount, IEnumerable<PetDto> PetsDtos), Error>> GetPetDto(GetPetsQuery request,
        CancellationToken token)
    {
        using var connection = _sqlConnectionFactory.Create();

        var sql = $"""
                   select count(*) from pets;

                   select p.volunteer_id                                       as VolunteerId,
                          concat_ws(' ', v.first_name, v.last_name, v.surname) as FullName,
                          p.name                                               as NickName,
                          s.name                                               as SpecieName,
                          b.name                                               as BreedName,
                          p.description                                        as Description,
                          p.color                                              as Color,
                          p.shared_health_status                               as SharedHealthStatus,
                          p.skin_condition                                     as SkinCondition,
                          p.mouth_condition                                    as MouthCondition,
                          p.digestive_system_condition                         as DigestiveSystemCondition,
                          p.country                                            as Country,
                          p.city                                               as City,
                          p.street                                             as Street,
                          p.house_number                                       as HouseNumber,
                          p.apartment_number                                   as ApartmentNumber,
                          p.height                                             as Height,
                          p.weight                                             as Weight,
                          concat_ws(' ', v.region_code, v.number)              as FullPhoneNumber,
                          p.sterilize                                          as Sterilize,
                          p.date_of_birth                                      as DateOfBirth,
                          p.vaccinated                                         as Vaccinated,
                          p.status                                             as HelpStatus,
                          p.details_for_help                                   as DetailsForHelps,
                          p.files                                              as FilesPet
                   from pets as p
                            left join volunteers as v on v.id = p.volunteer_id
                            left join species as s on s.id = p.species_id
                            left join breeds as b on b.id = p.breed_id
                            
                   """;


        var parameters = new DynamicParameters();
        var sb = new StringBuilder(sql);
        var sqlFilters = new List<string>();
        var sqlSort = new List<string>();

        if (request.VolunteerId != null)
        {
            parameters.Add("@VolunteerId", request.VolunteerId);
            sqlFilters.Add("p.volunteer_id = @VolunteerId");
        }

        if (String.IsNullOrWhiteSpace(request.NickName) == false)
        {
            parameters.Add("@NickName", request.NickName);
            sqlFilters.Add("p.name = @NickName");
        }

        if (String.IsNullOrWhiteSpace(request.Color) == false)
        {
            parameters.Add("@Color", request.Color);
            sqlFilters.Add("p.color ilike '%@Color%'");
        }

        if (request.Age != null)
        {
            parameters.Add("@Age", request.Age);
            sqlFilters.Add("p.age = @Age");
        }

        if (request.BreedId != null)
        {
            parameters.Add("@BreedId", request.BreedId);
            sqlFilters.Add("p.breed_id = @BreedId");
        }

        if (request.SpeciesId != null)
        {
            parameters.Add("@SpeciesId", request.SpeciesId);
            sqlFilters.Add("p.species_id = @SpeciesId");
        }

        if (String.IsNullOrWhiteSpace(request.Country) == false)
        {
            parameters.Add("@Country", request.Country);
            sqlFilters.Add("p.county = @Country");
        }

        if (request.SortByPetAge != null)
        {
            if (request.SortByPetAge.Asc == true)
            {
                sqlFilters.Add(" p.age asc ");
            }
            else
            {
                sqlFilters.Add(" p.age desc");
            }
        }

        if (request.SortByPetWeight == true)
        {
            sqlSort.Add(" p.weight ");
        }

        if (request.SortByPetNickName == true)
        {
            sqlSort.Add(" p.name ");
        }

        if (request.SortByPetDateOfBirth == true)
        {
            sqlSort.Add(" p.date_of_birth ");
        }


        if (sqlFilters.Any() == true)
        {
            sb.Append(" where ");
            sb.Append(string.Join(" and ", sqlFilters));
        }

        if (sqlSort.Any() == true)
        {
            sb.Append(" order by ");
            sb.Append(string.Join(" , ", sqlSort));
        }

        sb.Append(" offset @offset");
        sb.Append(" limit @limit;");

        parameters.AddPagination(request.Page, request.PageSize);

        var sqlQuery = sb.ToString();


        var command = new CommandDefinition(sqlQuery, parameters, cancellationToken: token);

        await using var result = await connection.QueryMultipleAsync(command);

        var petsCount = await result.ReadFirstAsync<long>();
        var petsDtos = result.Read<PetDto>();
        petsDtos = petsDtos.ToList();

        if (petsDtos.Any() == false)
        {
            return ErrorsPreform.General.NotFound("Nothing pets");
        }

        return (petsCount, petsDtos);
    }
}