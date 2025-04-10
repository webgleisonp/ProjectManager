using Project.Manager.Domain.Shared;

namespace Project.Manager.Application.Abstractions;

public interface IQueryHandler<TQuery, TResponse>
    where TQuery : IQuery
    where TResponse : class
{
    ValueTask<Result<TResponse>> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}