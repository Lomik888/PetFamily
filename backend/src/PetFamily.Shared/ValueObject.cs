namespace PetFamily.Shared;

public abstract class ValueObject
{
    protected abstract IEnumerable<object> GetEqualityComponents();

    public override bool Equals(object? obj)
    {
        if (obj is null || obj.GetType() != this.GetType())
        {
            return false;
        }

        var other = (ValueObject)obj;

        return this.GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
    }

    public override int GetHashCode()
    {
        return this.GetEqualityComponents().Aggregate(1, (current, obj) =>
            HashCode.Combine(current, obj?.GetHashCode() ?? 0));
    }

    public static bool operator ==(ValueObject left, ValueObject right)
    {
        if (object.ReferenceEquals(left, null) && object.ReferenceEquals(right, null))
        {
            return true;
        }

        if (object.ReferenceEquals(left, null) || object.ReferenceEquals(right, null))
        {
            return false;
        }

        return left.Equals(right);
    }

    public static bool operator !=(ValueObject left, ValueObject right) => !(left == right);
}