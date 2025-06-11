using CSharpFunctionalExtensions;
using PetFamily.SharedKernel.Errors;

namespace PetFamily.Core.Abstrations.Interfaces;

public interface IQueryHandler<TResultValue, TResultError, in TQuery>
    where TQuery : IQuery
    where TResultError : IEnumerable<Error>
{
    public Task<Result<TResultValue, TResultError>> Handle(
        TQuery request,
        CancellationToken cancellationToken = default);
}

public interface IQueryHandler<TResultError, in TQuery>
    where TQuery : IQuery
    where TResultError : IEnumerable<Error>
{
    public Task<UnitResult<TResultError>> Handle(
        TQuery request,
        CancellationToken cancellationToken = default);
}