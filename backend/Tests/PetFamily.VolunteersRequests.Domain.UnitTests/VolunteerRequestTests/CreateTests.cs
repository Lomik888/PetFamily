using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteersRequests.Domain.Dtos;
using PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

namespace PetFamily.VolunteersRequests.Domain.UnitTests.VolunteerRequestTests;

public class CreateTests
{
    [Fact]
    public void Create_volunteer_request_return_valid_entity()
    {
        var request = RequestVolunteerRequestBuilder.RequestVolunteerBuild();
        var volunteerRequest = VolunteerRequestFactory.CreateVolunteerRequest(request);

        var detailsForHelpCollection =
            request.DetailsForHelps.Select(x => DetailsForHelp.Create(x.Title, x.Description).Value);
        var detailsForHelps = DetailsForHelps.Create(detailsForHelpCollection).Value;
        var experience = Experience.Create(request.Experience).Value;

        var volunteerInfo = VolunteerInfo.Create(
            request.Certificates,
            experience,
            detailsForHelps).Value;
        var userId = UserId.Create(request.UserId).Value;
        var adminId = AdminId.Create(request.AdminId).Value;

        var dto = new CreateVolunteerRequestDto(
            adminId,
            userId,
            volunteerInfo);

        var sut = VolunteerRequest.CreateVolunteerRequest(dto);

        sut.Should().BeEquivalentTo(volunteerRequest, options =>
            options.Excluding(x => x.Id)
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.DiscussionId)
                .ComparingByMembers<VolunteerRequest>()
                .WithTracing());
    }
}