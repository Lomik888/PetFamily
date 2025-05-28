using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Application.VolunteerUseCases.Queries.GetPets;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Queries;

public class GetPetsHandlerTest : TestsBase
{
    private const int PAGE = 2;
    private const int PAGESIZE = 3;
    private const int COUNTVOLUNTEERS = 5;
    private const int COUNTPETS = 3;
    private const int COUNTSPECIES = 3;
    private const int COUNTBREEDS = 3;
    private IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery> _sut;

    public GetPetsHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<GetObjectsWithPaginationResponse<PetDto>, ErrorList, GetPetsQuery>>();
    }

    [Fact]
    public async Task Get_pets_handle_Result_should_be_true_and_valid_response()
    {
        var dapper = Scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>().Create();

        var cancellationToken = new CancellationToken();
        var (volunteers, species) = await SeedFullModelsAsync(
            COUNTVOLUNTEERS,
            COUNTPETS,
            COUNTSPECIES,
            COUNTBREEDS);

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

        var parameters = new DynamicParameters().AddPagination(PAGE, PAGESIZE);
        var sql = $"""
                   select count(*)
                   from pet ;

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
                          p.house_number                                       as HouseNumber,
                          p.apartment_number                                   as ApartmentNumber,
                          p.height                                             as Height,
                          p.weight                                             as Weight,
                          concat_ws(' ', v.region_code, v.number)              as FullPhoneNumber,
                          p.sterilize                                          as Sterilize,
                          p.date_of_birth                                      as DateOfBirth,
                          p.vaccinated                                         as Vaccinated,
                          p.details_for_help                                   as DetailsForHelps,
                          p.files                                              as FilesPet
                   from pets as p
                            left join volunteers as v on v.id = p.volunteer_id
                            left join species as s on s.id = p.species_id
                            left join breeds as b on b.id = p.breed_id
                   @offset
                   @limit
                   """;

        var multi = await dapper.QueryMultipleAsync(sql, parameters);
        var petsCount = await multi.ReadFirstOrDefaultAsync<long>();
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