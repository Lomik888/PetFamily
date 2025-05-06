using AutoFixture;
using PetFamily.Domain.UnitTests.AutoFixture;
using PetFamily.Domain.UnitTests.VolunteerTests.Fixtures;
using PetFamily.Domain.UnitTests.VolunteerTests.Requests;

namespace PetFamily.Domain.UnitTests.VolunteerTests.Builders;

public static class RequestVolunteerBuilder
{
    private static readonly Fixture _autoFixture = AutoFixtureBuilder.FixtureBuild();

    public static RequestVolunteer VolunteerBuild()
    {
        return _autoFixture
            .Build<RequestVolunteer>()
            .With(x => x.VolunteerId, Guid.NewGuid)
            .With(x => x.Email, "torak@mail.com")
            .With(x => x.Experience, 1)
            .With(x => x.RegionCode, "+7")
            .With(x => x.Number, "9535793109")
            .Create();
    }
}