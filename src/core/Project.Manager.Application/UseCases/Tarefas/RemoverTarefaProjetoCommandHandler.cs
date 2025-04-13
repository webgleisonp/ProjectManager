using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;

namespace Project.Manager.Application.UseCases.Tarefas;

internal sealed class RemoverTarefaProjetoCommandHandler(ITarefaRepository tarefaRepository, IUnityOfWork unityOfWork) : ICommandHandler<RemoverTarefaProjetoCommand>
{
    public async ValueTask<Result> HandleAsync(RemoverTarefaProjetoCommand command, CancellationToken cancellationToken = default)
    {
        var tarefa = await tarefaRepository.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), cancellationToken);

        if (tarefa is null)
            return Result.Failure<IncluirTarefaProjetoResponse>(TarefaErrors.TarefaNaoEncontrada);

        await tarefaRepository.RemoverTarefaProjetoAsync(tarefa, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
