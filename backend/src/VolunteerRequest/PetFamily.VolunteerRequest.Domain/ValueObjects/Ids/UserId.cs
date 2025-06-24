using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain;

public class UserId : BaseVoId
{
    protected UserId(Guid value) : base(value)
    {
    }
}