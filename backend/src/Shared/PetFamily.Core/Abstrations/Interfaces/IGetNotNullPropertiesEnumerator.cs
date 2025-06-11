namespace PetFamily.Core.Abstrations.Interfaces;

public interface IGetNotNullPropertiesEnumerator<TReturn>
{
    public IEnumerable<TReturn> GetNotNullPropertiesEnumerator();
}