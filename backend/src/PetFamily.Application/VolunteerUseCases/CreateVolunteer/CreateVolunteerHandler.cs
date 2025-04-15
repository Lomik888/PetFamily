using CSharpFunctionalExtensions;
using PetFamily.Domain.VolunteerContext.Entities;
using PetFamily.Domain.VolunteerContext.SharedVO;
using PetFamily.Domain.VolunteerContext.VolunteerVO;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.VolunteerUseCases.CreateVolunteer;

public class CreateVolunteerHandler : ICreateVolunteerHandler
{
    private readonly IVolunteerRepository _volunteerRepository;

    public CreateVolunteerHandler(IVolunteerRepository volunteerRepository)
    {
        _volunteerRepository = volunteerRepository;
    }

    public async Task<Result<Guid, Error[]>> Create(
        CreateVolunteerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var errors = new List<Error>();

        var id = VolunteerId.Create();

        var nameResult = Name.Create(request.Name.FirstName, request.Name.LastName, request.Name.Surname);
        if (nameResult.IsFailure)
        {
            errors.AddRange(nameResult.Error);
        }

        var emailResult = Email.Create(request.Email);
        if (emailResult.IsFailure)
        {
            errors.Add(emailResult.Error);
        }

        var descriptionResult = Description.Create(request.Description);
        if (descriptionResult.IsFailure)
        {
            errors.Add(descriptionResult.Error);
        }

        var experienceResult = Experience.Create(request.Experience);
        if (experienceResult.IsFailure)
        {
            errors.Add(experienceResult.Error);
        }

        var phoneNumberResult = PhoneNumber.Create(request.Phone.RegionCode, request.Phone.Number);
        if (phoneNumberResult.IsFailure)
        {
            errors.Add(phoneNumberResult.Error);
        }

        var socialNetworks = SocialNetworks.CreateEmpty().Value;
        var detailsForHelps = DetailsForHelps.CreateEmpty().Value;
        var files = Files.CreateEmpty().Value;
        IEnumerable<Pet> pets = [];

        if (errors.Count > 0)
        {
            return errors.ToArray();
        }

        var volunteer = new Volunteer(
            id,
            nameResult.Value,
            emailResult.Value,
            descriptionResult.Value,
            experienceResult.Value,
            phoneNumberResult.Value,
            socialNetworks,
            detailsForHelps,
            files,
            pets
        );

        await _volunteerRepository.AddAsync(volunteer, cancellationToken);
        await _volunteerRepository.SaveChangesAsync(cancellationToken);

        return volunteer.Id.Value;
    }
}