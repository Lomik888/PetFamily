using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.SharedVO;

public abstract class BaseCollectionVO<TCollection, TResult> : ValueObject
    where TResult : BaseCollectionVO<TCollection, TResult>
{
    protected List<TCollection> _items = [];

    public IReadOnlyList<TCollection> Items => _items;

    protected BaseCollectionVO()
    {
    }

    protected BaseCollectionVO(IEnumerable<TCollection> items)
    {
        _items = items.ToList();
    }

    public abstract Result<TResult, Error> Create(IEnumerable<TCollection> items);

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Items;
    }
}