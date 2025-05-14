namespace PetFamily.Application.Contracts.DTO.PetDtos;

public record AddressDto(
    string Country,
    string City,
    string Street,
    string HouseNumber,
    string ApartmentNumber);