using FluentAssertions;
using PetFamily.Domain.UnitTests.VolunteerTests.Builders;
using PetFamily.Domain.UnitTests.VolunteerTests.Fixtures;
using PetFamily.Domain.VolunteerContext.PetsVO;
using PetFamily.Shared.Errors.Enums;

namespace PetFamily.Domain.UnitTests.VolunteerTests;

public class MovePetTests
{
    [Fact]
    public void MovePet_WhenPetsCountIsZero_ReturnUnitResultIsFailureWithValidationTypeError()
    {
        var random = new Random();
        var numberForSerialNumber = random.Next(SerialNumber.INITIAL_VALUE, VolunteerFixture.COUNT_PET + 1);
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();

        var volunteerFixture = VolunteerFixture.CreateWithOutPets(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet);

        var volunteer = volunteerFixture.Volunteer;
        var volunteerForEqual = volunteerFixture.VolunteerForEqual;

        var someVolunteerWithOnePet = volunteerFixture.SomeVolunteerWithOnePet;
        var someVolunteerWithOnePetForEqual = volunteerFixture.SomeVolunteerWithOnePetForEqual;

        var somePet = volunteerFixture.SomePet;
        var somePetForEqual = volunteerFixture.SomePetForEqual;

        var serialNumber = SerialNumber.Create((uint)numberForSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)numberForSerialNumber).Value;

        var result = volunteer.MovePet(somePet, serialNumber);

        result.IsFailure.Should().BeTrue();
        volunteer.Should().BeEquivalentTo(volunteerForEqual);
        somePet.Should().BeEquivalentTo(somePetForEqual);
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        result.Error.ErrorType.Should().Be(ErrorType.VALIDATION);
    }

    [Fact]
    public void MovePet_WhenPetIdNotContentsInPets_ReturnUnitResultIsFailureWithValidationTypeError()
    {
        var random = new Random();
        var numberForSerialNumber = random.Next(SerialNumber.INITIAL_VALUE, VolunteerFixture.COUNT_PET + 1);
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();
        var requestPets = RequestPetBuilder.PetsBuild().ToList();

        var volunteerFixture = VolunteerFixture.Create(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            requestPets);

        var volunteer = volunteerFixture.Volunteer;
        var volunteerForEqual = volunteerFixture.VolunteerForEqual;

        var somePet = volunteerFixture.SomePet;
        var somePetForEqual = volunteerFixture.SomePetForEqual;

        var serialNumber = SerialNumber.Create((uint)numberForSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)numberForSerialNumber).Value;

        var result = volunteer.MovePet(somePet, serialNumber);

        result.IsFailure.Should().BeTrue();
        volunteer.Should().BeEquivalentTo(volunteerForEqual);
        somePet.Should().BeEquivalentTo(somePetForEqual);
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        result.Error.ErrorType.Should().Be(ErrorType.VALIDATION);
    }

    [Theory]
    [MemberData(
        nameof(MemberDatas
            .SerialNumbers_MovePet_WhenSerialNumberEqualPetsCountOrPetSerialNumber_ReturnUnitResultIsSuccess),
        MemberType = typeof(MemberDatas))]
    public void MovePet_WhenSerialNumberEqualPetSerialNumber_ReturnUnitResultIsSuccess(
        uint newSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();
        var requestPets = RequestPetBuilder.PetsBuild().ToList();

        var volunteerFixture = VolunteerFixture.Create(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            requestPets);

        var volunteer = volunteerFixture.Volunteer;
        var volunteerForEqual = volunteerFixture.VolunteerForEqual;

        var pet = volunteer.Pets.Single(x => x.SerialNumber.Value == newSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = volunteer.MovePet(pet, serialNumber);

        result.IsFailure.Should().BeFalse();
        volunteer.Should().BeEquivalentTo(volunteerForEqual);
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        result.Error.ErrorType.Should().Be(ErrorType.VALIDATION);
    }

    [Theory]
    [MemberData(
        nameof(MemberDatas
            .SerialNumbers_MovePet_WhenSerialNumberIsBiggerThenPetsCount_ReturnUnitResultIsSuccess),
        MemberType = typeof(MemberDatas))]
    public void SerialNumbers_MovePet_WhenSerialNumberIsBiggerThenPetsCount_ReturnUnitResultIsSuccess(
        uint oldSerialNumber, uint newSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();
        var requestPets = RequestPetBuilder.PetsBuild().ToList();

        var volunteerFixture = VolunteerFixture.Create(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            requestPets);

        var volunteer = volunteerFixture.Volunteer;
        var volunteerForEqual = volunteerFixture.VolunteerForEqual;

        var pet = volunteer.Pets.Single(x => x.SerialNumber.Value == oldSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = volunteer.MovePet(pet, serialNumber);

        var oldPets = volunteerForEqual.Pets
            .Where(x => x.SerialNumber.Value > oldSerialNumber)
            .ToList();
        var petsAfterMethod = volunteer.Pets
            .Where(x =>
                x.SerialNumber.Value >= oldSerialNumber &&
                x.SerialNumber.Value != volunteer.Pets.Count)
            .ToList();

        result.IsFailure.Should().BeFalse();
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        pet.SerialNumber.Value.Should().Be((uint)volunteer.Pets.Count);
        pet.SerialNumber.Value.Should().NotBe(newSerialNumber);
        volunteer.Should().BeEquivalentTo(volunteerForEqual, options =>
            options.Excluding(x => x.Path.Contains("Pets")));
        petsAfterMethod.Count.Should().Be(oldPets.Count);
        volunteer.Pets.Should().BeEquivalentTo(volunteerForEqual.Pets, options =>
            options.Excluding(x => x.SerialNumber));
        petsAfterMethod
            .Zip(oldPets, (after, before) => (after, before))
            .All(x => x.after.SerialNumber.Value == x.before.SerialNumber.Value - 1)
            .Should()
            .BeTrue();
    }

    [Theory]
    [MemberData(
        nameof(MemberDatas
            .SerialNumbers_MovePet_WhenSerialNumberIsLowerThanCurrent_ReturnUnitResultIsSuccess),
        MemberType = typeof(MemberDatas))]
    public void MovePet_WhenSerialNumberIsLowerThanCurrent_ReturnUnitResultIsSuccess(
        uint newSerialNumber,
        uint oldSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();
        var requestPets = RequestPetBuilder.PetsBuild().ToList();

        var volunteerFixture = VolunteerFixture.Create(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            requestPets);

        var volunteer = volunteerFixture.Volunteer;
        var volunteerForEqual = volunteerFixture.VolunteerForEqual;

        var pet = volunteer.Pets.Single(x => x.SerialNumber.Value == oldSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = volunteer.MovePet(pet, serialNumber);

        var oldPets = volunteerForEqual.Pets
            .Where(x =>
                x.SerialNumber.Value < oldSerialNumber &&
                x.SerialNumber.Value >= newSerialNumber)
            .ToList();
        var petsAfterMethod = volunteer.Pets
            .Where(x =>
                x.SerialNumber.Value > oldSerialNumber &&
                x.SerialNumber.Value <= newSerialNumber)
            .ToList();

        result.IsFailure.Should().BeFalse();
        pet.SerialNumber.Value.Should().Be(newSerialNumber);
        volunteer.Should().BeEquivalentTo(volunteerForEqual, options =>
            options.Excluding(x => x.Path.Contains("Pets")));
        volunteer.Pets.Should().BeEquivalentTo(volunteerForEqual.Pets, options =>
            options.Excluding(x => x.SerialNumber));
        petsAfterMethod
            .Zip(oldPets, (after, before) => (after, before))
            .All(x => x.after.SerialNumber.Value == x.before.SerialNumber.Value + 1)
            .Should()
            .BeTrue();
    }

    [Theory]
    [MemberData(
        nameof(MemberDatas
            .SerialNumbers_MovePet_WhenSerialNumberIsBiggerThanCurrent_ReturnUnitResultIsSuccess),
        MemberType = typeof(MemberDatas))]
    public void MovePet_WhenSerialNumberIsBiggerThanCurrent_ReturnUnitResultIsSuccess(
        uint newSerialNumber,
        uint oldSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();
        var requestPets = RequestPetBuilder.PetsBuild().ToList();

        var volunteerFixture = VolunteerFixture.Create(
            requestVolunteer,
            requestSomeVolunteer,
            requestSomePet,
            requestPets);

        var volunteer = volunteerFixture.Volunteer;
        var volunteerForEqual = volunteerFixture.VolunteerForEqual;

        var pet = volunteer.Pets.Single(x => x.SerialNumber.Value == oldSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = volunteer.MovePet(pet, serialNumber);

        var oldPets = volunteerForEqual.Pets
            .Where(x =>
                x.SerialNumber.Value > oldSerialNumber &&
                x.SerialNumber.Value <= newSerialNumber)
            .ToList();
        var petsAfterMethod = volunteer.Pets
            .Where(x =>
                x.SerialNumber.Value >= oldSerialNumber &&
                x.SerialNumber.Value < newSerialNumber)
            .ToList();

        result.IsFailure.Should().BeFalse();
        pet.SerialNumber.Value.Should().Be(newSerialNumber);
        volunteer.Should().BeEquivalentTo(volunteerForEqual, options =>
            options.Excluding(x => x.Path.Contains("Pets")));
        volunteer.Pets.Should().BeEquivalentTo(volunteerForEqual.Pets, options =>
            options.Excluding(x => x.SerialNumber));
        petsAfterMethod
            .Zip(oldPets, (after, before) => (after, before))
            .All(x => x.after.SerialNumber.Value == x.before.SerialNumber.Value - 1)
            .Should()
            .BeTrue();
    }
}