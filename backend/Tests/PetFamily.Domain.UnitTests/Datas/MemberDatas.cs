using PetFamily.Domain.UnitTests.VolunteerTests;
using PetFamily.Volunteers.Domain.ValueObjects.PetsVO;

namespace PetFamily.Domain.UnitTests.Datas;

public class MemberDatas
{
    public static IEnumerable<object[]>
        Serial_numbers_move_pet_when_serial_number_equal_pets_count_or_pet_serial_number_return_unit_result_issuccess()
    {
        var petCount = MovePetTests.COUNT_PETS;
        var rand = new Random();
        yield return new object[] { rand.Next(SerialNumber.INITIAL_VALUE, petCount + 1) };
        yield return new object[] { petCount };
    }

    public static IEnumerable<object[]>
        Serial_numbers_move_pet_when_serial_number_is_bigger_then_pets_count_return_unit_result_issuccess()
    {
        var petCount = MovePetTests.COUNT_PETS;
        var rand = new Random();
        yield return new object[]
        {
            rand.Next(SerialNumber.INITIAL_VALUE, petCount + 1),
            rand.Next(petCount, int.MaxValue)
        };
        yield return new object[]
        {
            rand.Next(SerialNumber.INITIAL_VALUE, petCount + 1),
            rand.Next(petCount, int.MaxValue)
        };
        yield return new object[]
        {
            rand.Next(SerialNumber.INITIAL_VALUE, petCount + 1),
            rand.Next(petCount, int.MaxValue)
        };
    }

    public static IEnumerable<object[]>
        Serial_numbers_move_pet_when_serial_number_is_lower_than_current_return_unit_result_issuccess()
    {
        yield return new object[] { 4, 9 };
        yield return new object[] { 1, 10 };
        yield return new object[] { 4, 10 };
        yield return new object[] { 1, 9 };
        yield return new object[] { 6, 7 };
        yield return new object[] { 9, 10 };
        yield return new object[] { 1, 2 };
    }

    public static IEnumerable<object[]>
        Serial_numbers_move_pet_when_serial_number_is_bigger_than_current_retur_nunit_result_issuccess()
    {
        yield return new object[] { 9, 4 };
        yield return new object[] { 10, 1 };
        yield return new object[] { 10, 4 };
        yield return new object[] { 9, 1 };
        yield return new object[] { 7, 6 };
        yield return new object[] { 10, 9 };
        yield return new object[] { 2, 1 };
    }
}