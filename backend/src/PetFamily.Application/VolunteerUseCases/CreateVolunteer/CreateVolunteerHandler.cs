using CSharpFunctionalExtensions;
using FluentValidation;
using Microsoft.Extensions.Logging;
using PetFamily.Application.Extensions;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.VolunteerUseCases.CreateVolunteer;

public class CreateVolunteerHandler : ICreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;
    private readonly IValidator<CreateVolunteerCommand> _validator;
    private readonly ILogger<CreateVolunteerHandler> _logger;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository,
        IValidator<CreateVolunteerCommand> validator, ILogger<CreateVolunteerHandler> logger)
    {
        _volunteerRepository = volunteerRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<Result<Guid, Error[]>> Create(
        CreateVolunteerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (validationResult.IsValid == false)
        {
            _logger.LogInformation("Invalid Validation request");
            return validationResult.Errors.ToErrors().ToArray();
        }

        var id = VolunteerId.Create();
        var nameResult = Name.Create(request.Name.FirstName, request.Name.LastName, request.Name.Surname).Value;
        var emailResult = Email.Create(request.Email).Value;
        var descriptionResult = Description.Create(request.Description).Value;
        var experienceResult = Experience.Create(request.Experience).Value;
        var phoneNumberResult = PhoneNumber.Create(request.Phone.RegionCode, request.Phone.Number).Value;
        var socialNetworks = SocialNetworks.CreateEmpty().Value;
        var detailsForHelps = DetailsForHelps.CreateEmpty().Value;
        var files = Files.CreateEmpty().Value;

        var volunteer = new Volunteer(
            id,
            nameResult,
            emailResult,
            descriptionResult,
            experienceResult,
            phoneNumberResult,
            socialNetworks,
            detailsForHelps,
            files
        );

        await _volunteerRepository.AddAsync(volunteer, cancellationToken);
        await _volunteerRepository.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Volunteer Created {0}", volunteer.Id.Value);
        return volunteer.Id.Value;
    }
}