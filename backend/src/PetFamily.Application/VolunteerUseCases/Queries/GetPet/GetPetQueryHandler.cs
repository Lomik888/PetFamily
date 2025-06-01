using CSharpFunctionalExtensions;
using Dapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Contracts;
using PetFamily.Application.Contracts.SharedInterfaces;
using PetFamily.Application.Extensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.VolunteerUseCases.Queries.GetPet;

public class GetPetQueryHandler : IQueryHandler<PetDto, ErrorList, GetPetQuery>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;
    private readonly ILogger<GetPetQueryHandler> _logger;
    private readonly IValidator<GetPetQuery> _validator;

    public GetPetQueryHandler(
        IValidator<GetPetQuery> validator,
        ILogger<GetPetQueryHandler> logger,
        ISqlConnectionFactory sqlConnectionFactory)
    {
        _validator = validator;
        _logger = logger;
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<PetDto, ErrorList>> Handle(
        GetPetQuery request,
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

        return result.Value;
    }

    private async Task<Result<PetDto, Error>> GetPetDto(GetPetQuery request, CancellationToken token)
    {
        using var connection = _sqlConnectionFactory.Create();

        var parameters = new DynamicParameters();

        parameters.Add("@id", request.PetId);

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

        var command = new CommandDefinition(sql, parameters, cancellationToken: token);

        var result = await connection.QuerySingleOrDefaultAsync<PetDto>(command);
        if (result == null)
        {
            return ErrorsPreform.General.NotFound(request.PetId);
        }

        return result;
    }
}