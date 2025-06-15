using CSharpFunctionalExtensions;

namespace PetFamily.SharedKernel.ValueObjects;

public abstract class ValueObjectList<T> : ValueObject
    where T : ValueObject
{
    public IReadOnlyList<T> Items { get; }

    public T this[int index] => Items[index];

    public int Count() => Items.Count;

    protected ValueObjectList()
    {
    }

    protected ValueObjectList(IEnumerable<T> items)
    {
        Items = items.ToList().AsReadOnly();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Items;
    }
}