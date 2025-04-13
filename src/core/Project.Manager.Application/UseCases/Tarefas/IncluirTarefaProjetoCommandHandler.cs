using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.UseCases.Tarefas;

internal sealed class IncluirTarefaProjetoCommandHandler(IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository, IUnityOfWork unityOfWork) : ICommandHandler<IncluirTarefaProjetoCommand, IncluirTarefaProjetoResponse>
{
    public async ValueTask<Result<IncluirTarefaProjetoResponse>> HandleAsync(IncluirTarefaProjetoCommand command, CancellationToken cancellationToken = default)
    {
        var projetoId = new ProjetoId(command.ProjetoId);

        var projeto = await projetoRepository.RetornarProjetoAsync(projetoId, cancellationToken);

        if (projeto is null)
            return Result.Failure<IncluirTarefaProjetoResponse>(ProjetoErrors.ProjetoNaoEncontrado);

        if (projeto.Tarefas.Count >= 20)
            return Result.Failure<IncluirTarefaProjetoResponse>(ProjetoErrors.LimiteDeTarefasPorProjeto);

        var tarefaResult = Tarefa.Criar(new TarefaId(Guid.NewGuid()), projeto.Id, command.Nome, command.Descricao, command.DataInicio, command.DataFim, StatusTarefa.Pendente, command.Prioridade);

        if (tarefaResult.IsFailure)
            return Result.Failure<IncluirTarefaProjetoResponse>(tarefaResult.Error);

        var tarefa = await tarefaRepository.AdicionarTarefaAsync(tarefaResult.Value, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(tarefa.ToIncluirTarefaProjetoResponse());
    }
}
