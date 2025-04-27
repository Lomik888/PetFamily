using PetFamily.Application.Contracts.DTO.VolunteerDtos;
using PetFamily.Application.Contracts.SharedInterfaces;

namespace PetFamily.Application.VolunteerUseCases.UpdateMainInfo;

public record UpdateMainInfoVolunteerCommand(
    Guid VolunteerId,
    NameUpdateDto? Name,
    string? Description,
    int? Experience) : ICommand, IGetNotNullPropertiesEnumerator<(string Name, object? Value)>
{
    public IEnumerable<(string Name, object? Value)> GetNotNullPropertiesEnumerator()
    {
        yield return (nameof(VolunteerId), VolunteerId);

        if (string.IsNullOrWhiteSpace(Description) == false)
            yield return (nameof(Description), Description);

        if (Experience is not null)
            yield return (nameof(Experience), Experience);

        if (Name is null) yield break;
        if (string.IsNullOrWhiteSpace(Name.FirstName) == false)
            yield return (nameof(Name.FirstName), Name.FirstName);

        if (string.IsNullOrWhiteSpace(Name.LastName) == false)
            yield return (nameof(Name.LastName), Name.LastName);

        if (string.IsNullOrWhiteSpace(Name.Surname) == false)
            yield return (nameof(Name.Surname), Name.Surname);
    }
}