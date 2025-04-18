﻿using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Validation;

namespace PetFamily.Domain.VolunteerContext.VolunteerVO;

public class Name : ValueObject
{
    private const int MIN_LENGTH = 1;
    public const int FIRST_NAME_LENGTH = 15;
    public const int LAST_NAME_LENGTH = 25;
    public const int SURNAME_LENGTH = 15;

    public string FirstName { get; }
    public string LastName { get; }
    public string Surname { get; }

    public string FullName() => $"{FirstName} {LastName} {Surname}";

    private Name(string firstName, string lastName, string surname)
    {
        FirstName = firstName;
        LastName = lastName;
        Surname = surname;
    }

    public static Result<Name, IEnumerable<Error>> Create(string firstName, string lastName, string surname)
    {
        var errors = new List<Error>();

        FieldValidator.ValidationField(firstName, MIN_LENGTH, FIRST_NAME_LENGTH, errors);
        FieldValidator.ValidationField(lastName, MIN_LENGTH, LAST_NAME_LENGTH, errors);
        FieldValidator.ValidationField(surname, MIN_LENGTH, SURNAME_LENGTH, errors);

        if (errors.Count > 0)
        {
            return errors.ToArray();
        }

        return new Name(firstName, lastName, surname);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FirstName;
        yield return LastName;
        yield return Surname;
    }
}