using PetFamily.Data.Tests.Requests;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteersRequests.Domain;
using PetFamily.VolunteersRequests.Domain.Dtos;
using PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

namespace PetFamily.Data.Tests.Factories;

public static class VolunteerRequestFactory
{
    public static VolunteerRequest CreateVolunteerRequest(RequestVolunteerRequest request)
    {
        var adminId = AdminId.Create(request.AdminId).Value;
        var userId = UserId.Create(request.UserId).Value;
        var volunteerInfo = VolunteerInfo.Create(
                request.Certificates,
                Experience.Create(request.Experience).Value,
                DetailsForHelps.Create(
                        request.DetailsForHelps
                            .Select(x =>
                                DetailsForHelp.Create(x.Title, x.Description).Value))
                    .Value)
            .Value;

        var dto = new CreateVolunteerRequestDto(
            adminId,
            userId,
            volunteerInfo);

        var volunteerRequest = VolunteerRequest.CreateVolunteerRequest(dto);
        return volunteerRequest;
    }
}