using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain;

public class DiscussionId : BaseVoId
{
    protected DiscussionId(Guid value) : base(value)
    {
    }
}