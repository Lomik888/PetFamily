namespace PetFamily.Shared.Results.Interfaces;

public interface IResult
{
    public bool IsSuccess { get; }

    public bool IsFailure { get; }
}