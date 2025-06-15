using FluentAssertions;
using PetFamily.Data.Tests.Builders;
using PetFamily.Data.Tests.Factories;
using PetFamily.Domain.UnitTests.Datas;
using PetFamily.SharedKernel.Errors.Enums;
using PetFamily.Volunteers.Domain;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Domain.UnitTests.VolunteerTests;

public class MovePetTests
{
    private readonly Random _random = new Random();
    public const int COUNT_PETS = 10;

    [Fact]
    public void Move_pet_when_pets_count_is_zero_return_unit_result_isFailure_with_validation_type_error()
    {
        var numberForSerialNumber = _random.Next(SerialNumber.INITIAL_VALUE, COUNT_PETS + 1);

        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();

        var volunteer = VolunteerFactory.CreateVolunteer(requestVolunteer);

        var sut = volunteer;
        var volunteerForEqual = (Volunteer)volunteer.Clone();

        var someVolunteerWithOnePet = VolunteerFactory.CreateVolunteer(requestSomeVolunteer);
        PetsFactory.CreatePet(someVolunteerWithOnePet, requestSomePet);

        var somePet = someVolunteerWithOnePet.Pets.First();
        var somePetForEqual = (Pet)somePet.Clone();

        var serialNumber = SerialNumber.Create((uint)numberForSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)numberForSerialNumber).Value;

        var result = sut.MovePet(somePet, serialNumber);

        result.IsFailure.Should().BeTrue();
        volunteer.Should().BeEquivalentTo(volunteerForEqual);
        somePet.Should().BeEquivalentTo(somePetForEqual);
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        result.Error.ErrorType.Should().Be(ErrorType.VALIDATION);
    }

    [Fact]
    public void Move_pet_when_pet_id_not_contents_in_pets_return_unit_result_is_failure_with_validation_type_error()
    {
        var numberForSerialNumber = _random.Next(SerialNumber.INITIAL_VALUE, COUNT_PETS + 1);

        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomeVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestSomePet = RequestPetBuilder.PetBuild();
        var requestPets = RequestPetBuilder.PetsBuild(COUNT_PETS).ToList();

        var volunteer = VolunteerFactory.CreateVolunteer(requestVolunteer);
        PetsFactory.CreatePets(volunteer, requestPets);

        var sut = volunteer;
        var volunteerForEqual = (Volunteer)volunteer.Clone();

        var someVolunteerWithOnePet = VolunteerFactory.CreateVolunteer(requestSomeVolunteer);
        PetsFactory.CreatePet(someVolunteerWithOnePet, requestSomePet);

        var somePet = someVolunteerWithOnePet.Pets.First();
        var somePetForEqual = (Pet)somePet.Clone();

        var serialNumber = SerialNumber.Create((uint)numberForSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)numberForSerialNumber).Value;

        var result = sut.MovePet(somePet, serialNumber);

        result.IsFailure.Should().BeTrue();
        volunteer.Should().BeEquivalentTo(volunteerForEqual);
        somePet.Should().BeEquivalentTo(somePetForEqual);
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        result.Error.ErrorType.Should().Be(ErrorType.VALIDATION);
    }

    [Theory]
    [MemberData(
        nameof(MemberDatas
            .Serial_numbers_move_pet_when_serial_number_equal_pets_count_or_pet_serial_number_return_unit_result_issuccess),
        MemberType = typeof(MemberDatas))]
    public void Move_pet_when_serial_number_equal_pet_serial_number_return_unit_result_is_success(
        uint newSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestPets = RequestPetBuilder.PetsBuild(COUNT_PETS).ToList();

        var volunteer = VolunteerFactory.CreateVolunteer(requestVolunteer);
        PetsFactory.CreatePets(volunteer, requestPets);

        var sut = volunteer;
        var volunteerForEqual = (Volunteer)volunteer.Clone();

        var pet = sut.Pets.Single(x => x.SerialNumber.Value == newSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = sut.MovePet(pet, serialNumber);

        result.IsFailure.Should().BeFalse();
        volunteer.Should().BeEquivalentTo(volunteerForEqual);
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
    }

    [Theory]
    [MemberData(
        nameof(MemberDatas
            .Serial_numbers_move_pet_when_serial_number_is_bigger_then_pets_count_return_unit_result_issuccess),
        MemberType = typeof(MemberDatas))]
    public void Serial_numbers_move_pet_when_serial_number_is_bigger_then_pets_count_return_unit_result_is_success(
        uint oldSerialNumber, uint newSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestPets = RequestPetBuilder.PetsBuild(COUNT_PETS).ToList();

        var volunteer = VolunteerFactory.CreateVolunteer(requestVolunteer);
        PetsFactory.CreatePets(volunteer, requestPets);

        var sut = volunteer;
        var volunteerForEqual = (Volunteer)volunteer.Clone();

        var pet = sut.Pets.Single(x => x.SerialNumber.Value == oldSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;
        var serialNumberForEqual = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = sut.MovePet(pet, serialNumber);

        var oldPets = volunteerForEqual.Pets
            .Where(x => x.SerialNumber.Value > oldSerialNumber)
            .ToList();
        var petsAfterMethod = sut.Pets
            .Where(x =>
                x.SerialNumber.Value >= oldSerialNumber &&
                x.SerialNumber.Value != sut.Pets.Count)
            .ToList();

        result.IsFailure.Should().BeFalse();
        serialNumber.Should().BeEquivalentTo(serialNumberForEqual);
        pet.SerialNumber.Value.Should().Be((uint)sut.Pets.Count);
        pet.SerialNumber.Value.Should().NotBe(newSerialNumber);
        sut.Should().BeEquivalentTo(volunteerForEqual, options =>
            options.Excluding(x => x.Path.Contains("Pets")));
        petsAfterMethod.Count.Should().Be(oldPets.Count);
        sut.Pets.Should().BeEquivalentTo(volunteerForEqual.Pets, options =>
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
            .Serial_numbers_move_pet_when_serial_number_is_lower_than_current_return_unit_result_issuccess),
        MemberType = typeof(MemberDatas))]
    public void Move_pet_when_serial_number_is_lower_than_current_return_unit_result_is_success(
        uint newSerialNumber,
        uint oldSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestPets = RequestPetBuilder.PetsBuild(COUNT_PETS).ToList();

        var volunteer = VolunteerFactory.CreateVolunteer(requestVolunteer);
        PetsFactory.CreatePets(volunteer, requestPets);

        var sut = volunteer;
        var volunteerForEqual = (Volunteer)volunteer.Clone();

        var pet = sut.Pets.Single(x => x.SerialNumber.Value == oldSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = sut.MovePet(pet, serialNumber);

        var oldPets = volunteerForEqual.Pets
            .Where(x =>
                x.SerialNumber.Value < oldSerialNumber &&
                x.SerialNumber.Value >= newSerialNumber)
            .ToList();
        var petsAfterMethod = sut.Pets
            .Where(x =>
                x.SerialNumber.Value > oldSerialNumber &&
                x.SerialNumber.Value <= newSerialNumber)
            .ToList();

        result.IsFailure.Should().BeFalse();
        pet.SerialNumber.Value.Should().Be(newSerialNumber);
        sut.Should().BeEquivalentTo(volunteerForEqual, options =>
            options.Excluding(x => x.Path.Contains("Pets")));
        sut.Pets.Should().BeEquivalentTo(volunteerForEqual.Pets, options =>
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
            .Serial_numbers_move_pet_when_serial_number_is_bigger_than_current_retur_nunit_result_issuccess),
        MemberType = typeof(MemberDatas))]
    public void Move_pet_when_serial_number_is_bigger_than_current_return_unit_result_is_success(
        uint newSerialNumber,
        uint oldSerialNumber)
    {
        var requestVolunteer = RequestVolunteerBuilder.VolunteerBuild();
        var requestPets = RequestPetBuilder.PetsBuild(COUNT_PETS).ToList();

        var volunteer = VolunteerFactory.CreateVolunteer(requestVolunteer);
        PetsFactory.CreatePets(volunteer, requestPets);

        var sut = volunteer;
        var volunteerForEqual = (Volunteer)volunteer.Clone();

        var pet = sut.Pets.Single(x => x.SerialNumber.Value == oldSerialNumber);

        var serialNumber = SerialNumber.Create((uint)newSerialNumber).Value;

        var result = sut.MovePet(pet, serialNumber);

        var oldPets = volunteerForEqual.Pets
            .Where(x =>
                x.SerialNumber.Value > oldSerialNumber &&
                x.SerialNumber.Value <= newSerialNumber)
            .ToList();
        var petsAfterMethod = sut.Pets
            .Where(x =>
                x.SerialNumber.Value >= oldSerialNumber &&
                x.SerialNumber.Value < newSerialNumber)
            .ToList();

        result.IsFailure.Should().BeFalse();
        pet.SerialNumber.Value.Should().Be(newSerialNumber);
        sut.Should().BeEquivalentTo(volunteerForEqual, options =>
            options.Excluding(x => x.Path.Contains("Pets")));
        sut.Pets.Should().BeEquivalentTo(volunteerForEqual.Pets, options =>
            options.Excluding(x => x.SerialNumber));
        petsAfterMethod
            .Zip(oldPets, (after, before) => (after, before))
            .All(x => x.after.SerialNumber.Value == x.before.SerialNumber.Value - 1)
            .Should()
            .BeTrue();
    }
}