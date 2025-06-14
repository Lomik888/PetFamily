using System.Text;
using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Core.Abstrations.Interfaces;
using PetFamily.Core.Extensions;
using PetFamily.Core;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.Errors;
using PetFamily.Volunteers.Application.Dtos.PetDtos;
using PetFamily.Volunteers.Application.Queries.GetPets;


namespace PetFamily.Application.IntegrationTests.VolunteersTests.Queries;

public class GetPetsQueryHandlerTest : TestsBase
{
    private const int PAGE = 2;
    private const int PAGESIZE = 3;
    private const int COUNT_VOLUNTEERS_MAX = 5;
    private const int COUNT_VOLUNTEERS_MIN = 2;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 5;
    private const int COUNT_SPECIES_MIN = 2;
    private const int COUNT_BREEDS_MAX = 5;
    private const int COUNT_BREEDS_MIN = 2;
    private IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery> _sut;

    public GetPetsQueryHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery>>();
    }

    [Fact]
    public async Task Get_pets_query_handle_Result_should_be_true_and_valid_response()
    {
        var cancellationToken = new CancellationToken();
        var (volunteers, species) = await DomainSeedFactory.SeedFullModelsAsync(
            TestDbContext,
            COUNT_VOLUNTEERS_MIN,
            COUNT_VOLUNTEERS_MAX,
            COUNT_PETS_MIN,
            COUNT_PETS_MAX,
            COUNT_SPECIES_MIN,
            COUNT_SPECIES_MAX,
            COUNT_BREEDS_MIN,
            COUNT_BREEDS_MAX);

        var query = new GetPetsQuery(
            PAGE,
            PAGESIZE,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            false,
            false,
            false
        );

        using var connection = SqlConnectionFactory.Create();
        var sql = $"""
                   select count(*) from "Volunteers".pets;

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
                   from "Volunteers".pets as p
                            left join "Volunteers".volunteers as v on v.id = p.volunteer_id
                            left join "Species".species as s on s.id = p.species_id
                            left join "Species".breeds as b on b.id = p.breed_id
                            
                   """;


        var parameters = new DynamicParameters();
        var sb = new StringBuilder(sql);
        var sqlFilters = new List<string>();
        var sqlSort = new List<string>();

        if (query.VolunteerId != null)
        {
            parameters.Add("@VolunteerId", query.VolunteerId);
            sqlFilters.Add("p.volunteer_id = @VolunteerId");
        }

        if (String.IsNullOrWhiteSpace(query.NickName) == false)
        {
            parameters.Add("@NickName", query.NickName);
            sqlFilters.Add("p.name = @NickName");
        }

        if (String.IsNullOrWhiteSpace(query.Color) == false)
        {
            parameters.Add("@Color", query.Color);
            sqlFilters.Add("p.color ilike '%@Color%'");
        }

        if (query.Age != null)
        {
            parameters.Add("@Age", query.Age);
            sqlFilters.Add("p.age = @Age");
        }

        if (query.BreedId != null)
        {
            parameters.Add("@BreedId", query.BreedId);
            sqlFilters.Add("p.breed_id = @BreedId");
        }

        if (query.SpeciesId != null)
        {
            parameters.Add("@SpeciesId", query.SpeciesId);
            sqlFilters.Add("p.species_id = @SpeciesId");
        }

        if (String.IsNullOrWhiteSpace(query.Country) == false)
        {
            parameters.Add("@Country", query.Country);
            sqlFilters.Add("p.county = @Country");
        }

        if (query.SortByPetAge != null)
        {
            if (query.SortByPetAge.Asc == true)
            {
                sqlFilters.Add(" p.age asc ");
            }
            else
            {
                sqlFilters.Add(" p.age desc");
            }
        }

        if (query.SortByPetWeight == true)
        {
            sqlSort.Add(" p.weight ");
        }

        if (query.SortByPetNickName == true)
        {
            sqlSort.Add(" p.name ");
        }

        if (query.SortByPetDateOfBirth == true)
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

        parameters.AddPagination(query.Page, query.PageSize);

        var sqlQuery = sb.ToString();


        var command = new CommandDefinition(sqlQuery, parameters, cancellationToken: cancellationToken);

        await using var multi = await connection.QueryMultipleAsync(command);

        var petsCount = await multi.ReadFirstAsync<long>();
        var petsDtos = multi.Read<PetDto>();

        var handleResultSuccessValue = new GetObjectsWithPaginationResponse<PetDto>()
        {
            Data = petsDtos,
            Page = PAGE,
            PageSize = PAGESIZE,
            Count = petsCount,
        };

        var result = await _sut.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(handleResultSuccessValue);
    }
}