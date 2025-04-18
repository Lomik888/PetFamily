using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;

namespace PetFamily.Domain.SharedVO;

public abstract class BaseCollectionVO<TCollection, TResult> : ValueObject
    where TResult : BaseCollectionVO<TCollection, TResult>
{
    public IReadOnlyList<TCollection> Items { get; }

    protected BaseCollectionVO()
    {
    }

    protected BaseCollectionVO(IEnumerable<TCollection> items)
    {
        Items = items.ToList();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Items;
    }
}