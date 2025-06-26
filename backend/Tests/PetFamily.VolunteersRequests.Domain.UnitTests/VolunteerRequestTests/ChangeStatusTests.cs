using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.VolunteersRequests.Domain.ValueObjects;

namespace PetFamily.VolunteersRequests.Domain.UnitTests.VolunteerRequestTests;

public class ChangeStatusTests
{
    [Fact]
    public void On_review__volunteer_request_return_valid_entity()
    {
        var request = RequestVolunteerRequestBuilder.RequestVolunteerBuild();
        var volunteerRequest = VolunteerRequestFactory.CreateVolunteerRequest(request);
        var sut = (VolunteerRequest)volunteerRequest.Clone();

        volunteerRequest.OnReviewVolunteerRequest();
        sut.OnReviewVolunteerRequest();

        sut.Should().BeEquivalentTo(volunteerRequest, options =>
            options.Excluding(x => x.Id)
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.DiscussionId)
                .ComparingByMembers<VolunteerRequest>()
                .WithTracing());
    }

    [Fact]
    public void Approved_volunteer_request_return_valid_entity()
    {
        var request = RequestVolunteerRequestBuilder.RequestVolunteerBuild();
        var volunteerRequest = VolunteerRequestFactory.CreateVolunteerRequest(request);
        var sut = (VolunteerRequest)volunteerRequest.Clone();

        volunteerRequest.ApprovedVolunteerRequest();
        sut.ApprovedVolunteerRequest();

        sut.Should().BeEquivalentTo(volunteerRequest, options =>
            options.Excluding(x => x.Id)
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.DiscussionId)
                .ComparingByMembers<VolunteerRequest>()
                .WithTracing());
    }

    [Fact]
    public void Revision_required_volunteer_request_return_valid_entity()
    {
        var request = RequestVolunteerRequestBuilder.RequestVolunteerBuild();
        var volunteerRequest = VolunteerRequestFactory.CreateVolunteerRequest(request);
        var sut = (VolunteerRequest)volunteerRequest.Clone();
        var comment = "Всем кискам кис it's rejectionComment on Revision method";
        var rejectionComment = RejectionComment.Create(comment).Value;

        volunteerRequest.RevisionRequiredVolunteerRequest(rejectionComment);
        sut.RevisionRequiredVolunteerRequest(rejectionComment);

        sut.Should().BeEquivalentTo(volunteerRequest, options =>
            options.Excluding(x => x.Id)
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.DiscussionId)
                .ComparingByMembers<VolunteerRequest>()
                .WithTracing());
    }

    [Fact]
    public void Rejected_volunteer_request_return_valid_entity()
    {
        var request = RequestVolunteerRequestBuilder.RequestVolunteerBuild();
        var volunteerRequest = VolunteerRequestFactory.CreateVolunteerRequest(request);
        var sut = (VolunteerRequest)volunteerRequest.Clone();
        var comment = "Всем кискам кис it's rejectionComment on Rejected method";
        var rejectionComment = RejectionComment.Create(comment).Value;

        volunteerRequest.RevisionRequiredVolunteerRequest(rejectionComment);
        sut.RevisionRequiredVolunteerRequest(rejectionComment);

        sut.Should().BeEquivalentTo(volunteerRequest, options =>
            options.Excluding(x => x.Id)
                .Excluding(x => x.CreatedAt)
                .Excluding(x => x.DiscussionId)
                .ComparingByMembers<VolunteerRequest>()
                .WithTracing());
    }
}