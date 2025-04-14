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

    public async Task<Result<Guid, Error>> Create(
        CreateVolunteerCommand request,
        CancellationToken cancellationToken = default
    )
    {
        var id = VolunteerId.Create();
        var name = Name.Create(request.Name.FirstName, request.Name.LastName, request.Name.Surname).Value;
        var email = Email.Create(request.Email).Value;
        var description = Description.Create(request.Description).Value;
        var experience = Experience.Create(request.Experience).Value;
        var phoneNumber = PhoneNumber.Create(request.Phone.RegionCode, request.Phone.Number).Value;
        var socialNetworks = SocialNetworks.CreateEmpty().Value;
        var detailsForHelps = DetailsForHelps.CreateEmpty().Value;
        var files = Files.CreateEmpty().Value;
        IEnumerable<Pet> pets = [];

        var volunteer = new Volunteer(
            id,
            name,
            email,
            description,
            experience,
            phoneNumber,
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