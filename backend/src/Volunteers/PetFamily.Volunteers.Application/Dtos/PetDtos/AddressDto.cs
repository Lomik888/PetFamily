namespace PetFamily.Volunteers.Application.Dtos.PetDtos;

public record AddressDto(
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string ApartmentNumber);