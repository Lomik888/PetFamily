using System.Reflection;
using AutoFixture.Kernel;

namespace PetFamily.Domain.UnitTests.AutoFixture;

public class PropertyNameSpecimenBuilder : ISpecimenBuilder
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType == typeof(string))
            {
                return propertyInfo.Name;
            }
        }

        return new NoSpecimen();
    }
}