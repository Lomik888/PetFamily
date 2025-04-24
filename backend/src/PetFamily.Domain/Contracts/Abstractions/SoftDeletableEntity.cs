using CSharpFunctionalExtensions;
using PetFamily.Domain.SharedVO;

namespace PetFamily.Domain.Contracts.Abstractions;

public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    public bool IsActive { get; protected set; } = true;
    public DeletedAt? DeletedAt { get; protected set; } = null;

    protected SoftDeletableEntity(TId id) :
        base(id)
    {
    }

    public virtual void UnActivate()
    {
        if (IsActive == false)
        {
            return;
        }

        DeletedAt = DeletedAt.Create(DateTime.UtcNow).Value;
        IsActive = false;
    }

    public virtual void Activate()
    {
        if (IsActive == true)
        {
            return;
        }

        DeletedAt = null;
        IsActive = false;
    }
}