using Dapper;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.VolunteerUseCases.Queries.GetPet;
using PetFamily.Data.Tests.Factories;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.IntegrationTests.VolunteersTests.Queries;

public class GetPetQueryHandlerTest : TestsBase
{
    private const int COUNT_VOLUNTEERS_MAX = 5;
    private const int COUNT_VOLUNTEERS_MIN = 2;
    private const int COUNT_PETS_MAX = 10;
    private const int COUNT_PETS_MIN = 2;
    private const int COUNT_SPECIES_MAX = 5;
    private const int COUNT_SPECIES_MIN = 2;
    private const int COUNT_BREEDS_MAX = 5;
    private const int COUNT_BREEDS_MIN = 2;
    private readonly IQueryHandler<PetDto, ErrorList, GetPetQuery> _sut;

    public GetPetQueryHandlerTest(
        IntegrationsTestsWebAppFactory factory) : base(factory)
    {
        _sut = Scope.ServiceProvider
            .GetRequiredService<IQueryHandler<PetDto, ErrorList, GetPetQuery>>();
    }

    [Fact]
    public async Task Get_pet_query_handle_Result_should_be_true_and_valid_response()
    {
        var cancellationToken = new CancellationToken();

        var (volunteers, species) = await DomainSeedFactory.SeedFullModelsAsync(
            DbContext,
            COUNT_VOLUNTEERS_MIN,
            COUNT_VOLUNTEERS_MAX,
            COUNT_PETS_MIN,
            COUNT_PETS_MAX,
            COUNT_SPECIES_MIN,
            COUNT_SPECIES_MAX,
            COUNT_BREEDS_MIN,
            COUNT_BREEDS_MAX);

        var indexVolunteer = Random.Next(volunteers.Count);
        var volunteer = volunteers[indexVolunteer];

        var indexPet = Random.Next(volunteer.Pets.Count);
        var pet = volunteer.Pets[indexPet];

        var query = new GetPetQuery(pet.Id.Value);

        using var connection = SqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        parameters.Add("@id", query.PetId);

        var sql = $"""
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
                   where p.id = @id;
                   """;

        var command = new CommandDefinition(sql, parameters);

        var resultTest = await connection.QuerySingleOrDefaultAsync<PetDto>(command);

        var result = await _sut.Handle(query, cancellationToken);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(resultTest);
    }
}