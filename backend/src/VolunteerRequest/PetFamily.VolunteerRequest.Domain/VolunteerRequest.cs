using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain;

public sealed class VolunteerRequest : Entity<VolunteerRequestId>
{
    public DiscussionId DiscussionId { get; private set; }
    public CreatedAt CreatedAt { get; private set; }
    public RejectionComment? RejectionComment { get; private set; }
    public StatusVolunteerRequest StatusVolunteerRequest { get; private set; }
    public AdminId AdminId { get; private set; }
    public UserId UserId { get; private set; }
    public VolunteerInfo VolunteerInfo { get; private set; }

    private VolunteerRequest()
    {
    }

    private VolunteerRequest(
        VolunteerRequestId volunteerRequestId,
        DiscussionId discussionId,
        CreatedAt createdAt,
        RejectionComment? rejectionComment,
        StatusVolunteerRequest statusVolunteerRequest,
        AdminId adminId,
        UserId userId,
        VolunteerInfo volunteerInfo)
    {
        Id = volunteerRequestId;
        DiscussionId = discussionId;
        CreatedAt = createdAt;
        RejectionComment = rejectionComment;
        StatusVolunteerRequest = statusVolunteerRequest;
        AdminId = adminId;
        UserId = userId;
        VolunteerInfo = volunteerInfo;
    }

    public void CreateVolunteerRequest()
    {
    }

    public void OnReviewVolunteerRequest()
    {
    }

    public void RevisionRequiredVolunteerRequest()
    {
    }

    public void ApprovedVolunteerRequest()
    {
    }

    public void RejectedVolunteerRequest()
    {
    }
}