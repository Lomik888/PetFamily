using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Application.VolunteerUseCases.CreateVolunteer;

public interface ICreateVolunteerHandler
{
    public Task<Result<Guid, Error[]>> Create(
        CreateVolunteerCommand request,
        CancellationToken cancellationToken = default);
}