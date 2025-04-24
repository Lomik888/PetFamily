namespace PetFamily.Application.Contracts.SharedInterfaces;

public interface IGetNotNullPropertiesEnumerator<TReturn>
{
    public IEnumerable<TReturn> GetNotNullPropertiesEnumerator();
}