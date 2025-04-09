using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.PetsVO;

public class Address : ValueObject
{
    private const int MIN_LENGHT = 1;
    private const int COUNTRY_MAX_LENGHT = 75;
    private const int CITY_MAX_LENGHT = 50;
    private const int STREET_MAX_LENGHT = 50;
    private const int HOUSENUMBER_MAX_LENGHT = 5;
    private const int APARTMENTNUMBER_MAX_LENGHT = 5;

    public string Country { get; }
    public string City { get; }
    public string Street { get; }
    public string HouseNumber { get; }
    public string ApartmentNumber { get; }

    public string FullAddress => $"{Country} {City} {Street} {HouseNumber} {ApartmentNumber}";

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

    public static Result<Address, Error[]> Create(
        string country,
        string city,
        string street,
        string houseNumber,
        string? apartmentNumber)
    {
        var errors = new List<Error>();

        FieldValidator.ValidationField(country, MIN_LENGHT, COUNTRY_MAX_LENGHT, errors);
        FieldValidator.ValidationField(city, MIN_LENGHT, CITY_MAX_LENGHT, errors);
        FieldValidator.ValidationField(street, MIN_LENGHT, STREET_MAX_LENGHT, errors);
        FieldValidator.ValidationField(houseNumber, MIN_LENGHT, HOUSENUMBER_MAX_LENGHT, errors);
        FieldValidator.ValidationNullableField(apartmentNumber, MIN_LENGHT, APARTMENTNUMBER_MAX_LENGHT, errors);

        if (errors.Count > 0)
        {
            return errors.ToArray();
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