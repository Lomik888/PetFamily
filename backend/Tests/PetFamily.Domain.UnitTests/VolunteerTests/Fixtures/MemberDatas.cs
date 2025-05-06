using PetFamily.Domain.VolunteerContext.PetsVO;

namespace PetFamily.Domain.UnitTests.VolunteerTests.Fixtures;

public class MemberDatas
{
    public static IEnumerable<object[]>
        SerialNumbers_MovePet_WhenSerialNumberEqualPetsCountOrPetSerialNumber_ReturnUnitResultIsSuccess()
    {
        var rand = new Random();
        yield return new object[] { rand.Next(SerialNumber.INITIAL_VALUE, VolunteerFixture.COUNT_PET + 1) };
        yield return new object[] { VolunteerFixture.COUNT_PET };
    }

    public static IEnumerable<object[]>
        SerialNumbers_MovePet_WhenSerialNumberIsBiggerThenPetsCount_ReturnUnitResultIsSuccess()
    {
        var rand = new Random();
        yield return new object[]
        {
            rand.Next(SerialNumber.INITIAL_VALUE, VolunteerFixture.COUNT_PET + 1),
            rand.Next(VolunteerFixture.COUNT_PET, int.MaxValue)
        };
        yield return new object[]
        {
            rand.Next(SerialNumber.INITIAL_VALUE, VolunteerFixture.COUNT_PET + 1),
            rand.Next(VolunteerFixture.COUNT_PET, int.MaxValue)
        };
        yield return new object[]
        {
            rand.Next(SerialNumber.INITIAL_VALUE, VolunteerFixture.COUNT_PET + 1),
            rand.Next(VolunteerFixture.COUNT_PET, int.MaxValue)
        };
    }

    public static IEnumerable<object[]>
        SerialNumbers_MovePet_WhenSerialNumberIsLowerThanCurrent_ReturnUnitResultIsSuccess()
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
        SerialNumbers_MovePet_WhenSerialNumberIsBiggerThanCurrent_ReturnUnitResultIsSuccess()
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