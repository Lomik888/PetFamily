using CSharpFunctionalExtensions;
using PetFamily.Shared.Errors;
using PetFamily.Shared.Errors.Interfaces;

namespace PetFamily.Application.Contracts.SharedInterfaces;

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