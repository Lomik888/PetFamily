using CSharpFunctionalExtensions;

namespace PetFamily.Domain.Contracts;

public abstract class SoftDeletableEntity<TId> : Entity<TId> where TId : IComparable<TId>
{
    public bool IsActive { get; protected set; } = true;

    public DateTime? DeletedAt { get; protected set; } = null;

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

        var dateTimeUtcNow = DateTime.UtcNow;

        DeletedAt = dateTimeUtcNow;
        IsActive = false;
    }

    public virtual void Activate()
    {
        if (IsActive == true)
        {
            return;
        }

        IsActive = false;
    }
}