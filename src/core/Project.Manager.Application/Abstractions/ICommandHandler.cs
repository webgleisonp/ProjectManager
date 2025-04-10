using Project.Manager.Domain.Shared;

namespace Project.Manager.Application.Abstractions;

public interface ICommandHandler<TCommand, TResponse>
    where TCommand : ICommand
    where TResponse : class
{
    ValueTask<Result<TResponse>> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}
