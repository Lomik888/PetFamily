using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Core.Abstrations.Interfaces;

public interface ICommandHandler<TResultValue, TResultError, in TCommand>
    where TCommand : ICommand
    where TResultError : IEnumerable<Error>
{
    public Task<Result<TResultValue, TResultError>> Handle(
        TCommand request,
        CancellationToken cancellationToken = default);
}

public interface ICommandHandler<TResultError, in TCommand>
    where TCommand : ICommand
    where TResultError : IEnumerable<Error>
{
    public Task<UnitResult<TResultError>> Handle(
        TCommand request,
        CancellationToken cancellationToken = default);
}