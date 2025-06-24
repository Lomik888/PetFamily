using PetFamily.SharedKernel.ValueObjects;

namespace PetFamily.VolunteerRequest.Domain;

public class AdminId : BaseVoId
{
    protected AdminId(Guid value) : base(value)
    {
    }
}