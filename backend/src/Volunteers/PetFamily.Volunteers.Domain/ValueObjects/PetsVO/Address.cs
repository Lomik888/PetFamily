using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;
using PetFamily.SharedKernel.Validation;

namespace PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

public class Address : ValueObject
{
    public const int MIN_LENGHT = 1;
    public const int COUNTRY_MAX_LENGHT = 75;
    public const int CITY_MAX_LENGHT = 50;
    public const int STREET_MAX_LENGHT = 50;
    public const int HOUSENUMBER_MAX_LENGHT = 5;
    public const int APARTMENTNUMBER_MAX_LENGHT = 5;

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string HouseNumber { get; }
    public string ApartmentNumber { get; }

    public string FullAddress() => $"{Country} {City} {Street} {HouseNumber} {ApartmentNumber}";

    private Address(
        string country,
        string city,
        string street,
        string houseNumber,
        string apartmentNumber)
    {
        Country = country;
        City = city;
        Street = street;
        HouseNumber = houseNumber;
        ApartmentNumber = apartmentNumber;
    }

    public static Result<Address, ErrorList> Create(
        string country,
        string city,
        string street,
        string houseNumber,
        string? apartmentNumber)
    {
        var errors = new List<Error>();

        Validator.FieldValueObject.Validation(country, MIN_LENGHT, COUNTRY_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(city, MIN_LENGHT, CITY_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(street, MIN_LENGHT, STREET_MAX_LENGHT, errors);
        Validator.FieldValueObject.Validation(houseNumber, MIN_LENGHT, HOUSENUMBER_MAX_LENGHT, errors);
        Validator.FieldValueObject.ValidationNullable(apartmentNumber, MIN_LENGHT, APARTMENTNUMBER_MAX_LENGHT, errors);

        if (errors.Count > 0)
        {
            return ErrorList.Create(errors);
        }

        return new Address(
            country,
            city,
            street,
            houseNumber,
            apartmentNumber ?? string.Empty);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Country;
        yield return City;
        yield return Street;
        yield return HouseNumber;
        yield return ApartmentNumber;
    }
}