using AutoFixture;

namespace PetFamily.Domain.UnitTests.AutoFixture;

public class AutoFixtureBuilder : Fixture
{
    public static Fixture FixtureBuild()
    {
        var fixture = new Fixture();
        fixture.Customizations.Add(new PropertyNameSpecimenBuilder());
        return fixture;
    }
}