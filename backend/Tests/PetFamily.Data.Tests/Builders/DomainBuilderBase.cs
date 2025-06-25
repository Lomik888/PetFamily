using AutoFixture;

namespace PetFamily.Data.Tests.Builders;

public class DomainBuilderBase
{
    protected static readonly Fixture _autoFixture = new Fixture();
    protected static readonly Random _random = new Random();
}