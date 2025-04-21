namespace PetFamily.Application.SharedInterfaces;

public interface IGetNotNullPropertiesEnumerator<TReturn>
{
    public IEnumerable<TReturn> GetNotNullPropertiesEnumerator();
}