using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.UseCases.Tarefas;

internal sealed class RemoverTarefaProjetoCommandHandler(ITarefaRepository tarefaRepository, IUnityOfWork unityOfWork) : ICommandHandler<RemoverTarefaProjetoCommand>
{
    public async ValueTask<Result> HandleAsync(RemoverTarefaProjetoCommand command, CancellationToken cancellationToken = default)
    {
        var tarefa = await tarefaRepository.RetornarTarefaAsync(new TarefaId(command.TarefaId), cancellationToken);

        if (tarefa is null)
            return Result.Failure<IncluirTarefaProjetoResponse>(TarefaErrors.TarefaNaoEncontrada);

        await tarefaRepository.RemoverTarefaProjetoAsync(tarefa, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
