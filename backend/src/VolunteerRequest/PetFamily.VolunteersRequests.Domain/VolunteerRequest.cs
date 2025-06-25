using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.ValueObjects;
using PetFamily.VolunteersRequests.Domain.Dtos;
using PetFamily.VolunteersRequests.Domain.Enums;
using PetFamily.VolunteersRequests.Domain.ValueObjects;
using PetFamily.VolunteersRequests.Domain.ValueObjects.Ids;

namespace PetFamily.VolunteersRequests.Domain;

public sealed class VolunteerRequest : Entity<VolunteerRequestId>, ICloneable
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

    public static VolunteerRequest CreateVolunteerRequest(CreateVolunteerRequestDto dto)
    {
        var volunteerRequestId = VolunteerRequestId.Create();
        var discussionId = DiscussionId.Create();
        var createdAt = CreatedAt.UtcNow();
        var statusVolunteerRequest = StatusVolunteerRequest.Sumbitted;

        var volunteerRequest = new VolunteerRequest(
            volunteerRequestId,
            discussionId,
            createdAt,
            null,
            statusVolunteerRequest,
            dto.AdminId,
            dto.UserId,
            dto.VolunteerInfo);

        return volunteerRequest;
    }

    public void OnReviewVolunteerRequest()
    {
        var newStatus = StatusVolunteerRequest.OnReview;
        this.StatusVolunteerRequest = newStatus;
    }

    public void RevisionRequiredVolunteerRequest(RejectionComment rejectionComment)
    {
        var newStatus = StatusVolunteerRequest.RevisionRequired;
        this.StatusVolunteerRequest = newStatus;
        this.RejectionComment = rejectionComment;
    }

    public void ApprovedVolunteerRequest()
    {
        var newStatus = StatusVolunteerRequest.Approved;
        this.StatusVolunteerRequest = newStatus;
    }

    public void RejectedVolunteerRequest(RejectionComment rejectionComment)
    {
        var newStatus = StatusVolunteerRequest.Rejected;
        this.StatusVolunteerRequest = newStatus;
        this.RejectionComment = rejectionComment;
    }

    public object Clone()
    {
        var id = VolunteerRequestId.Create(this.Id.Value).Value;
        var discussionId = DiscussionId.Create(this.DiscussionId.Value).Value;
        var createdAt = CreatedAt.Create(this.CreatedAt.Value).Value;
        var rejectionComment = this.RejectionComment == null
            ? null
            : RejectionComment.Create(this.RejectionComment.Value).Value;
        var statusVolunteerRequest = this.StatusVolunteerRequest;
        var adminId = AdminId.Create(this.AdminId.Value).Value;
        var userId = UserId.Create(this.UserId.Value).Value;
        var volunteerInfo = VolunteerInfo.Create(
            this.VolunteerInfo.Certificates,
            this.VolunteerInfo.Experience,
            this.VolunteerInfo.DetailsForHelps).Value;

        var volunteerRequest = new VolunteerRequest(
            id,
            discussionId,
            createdAt,
            rejectionComment,
            statusVolunteerRequest,
            adminId,
            userId,
            volunteerInfo);

        return volunteerRequest;
    }
}