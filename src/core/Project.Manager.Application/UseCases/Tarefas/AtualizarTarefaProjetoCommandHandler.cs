using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record AtualizarTarefaProjetoResponse(Guid TarefaId, string Projeto, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim, StatusTarefa Status, PrioridadeTarefa Prioridade);

internal sealed class AtualizarTarefaProjetoCommandHandler(IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository, IUnityOfWork unityOfWork) : ICommandHandler<AtualizarTarefaProjetoCommand, AtualizarTarefaProjetoResponse>
{
    public async ValueTask<Result<AtualizarTarefaProjetoResponse>> HandleAsync(AtualizarTarefaProjetoCommand command, CancellationToken cancellationToken = default)
    {
        var projeto = await projetoRepository.RetornarProjetoAsync(command.ProjetoId.ToProjetoId(), cancellationToken);

        if (projeto is null)
            return Result.Failure<AtualizarTarefaProjetoResponse>(ProjetoErrors.ProjetoNaoEncontrado);

        if (projeto.Tarefas.Count >= 20)
            return Result.Failure<AtualizarTarefaProjetoResponse>(ProjetoErrors.LimiteDeTarefasPorProjeto);

        var tarefa = await tarefaRepository.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), cancellationToken);

        if (tarefa is null)
            return Result.Failure<AtualizarTarefaProjetoResponse>(TarefaErrors.TarefaNaoEncontrada);

        var setNomeResult = tarefa.SetNome(command.Nome);

        if (setNomeResult.IsFailure)
            return Result.Failure<AtualizarTarefaProjetoResponse>(setNomeResult.Error);

        var setDescricaoResult = tarefa.SetDescricao(command.Descricao);

        if (setDescricaoResult.IsFailure)
            return Result.Failure<AtualizarTarefaProjetoResponse>(setDescricaoResult.Error);

        var setDataInicioResult = tarefa.SetDataInicio(command.DataInicio);

        if (setDataInicioResult.IsFailure)
            return Result.Failure<AtualizarTarefaProjetoResponse>(setDataInicioResult.Error);

        var setDataFimResult = tarefa.SetDataFim(command.DataFim);

        if (setDataFimResult.IsFailure)
            return Result.Failure<AtualizarTarefaProjetoResponse>(setDataFimResult.Error);

        var setStatusResult = tarefa.SetStatus(command.Status);

        if (setStatusResult.IsFailure)
            return Result.Failure<AtualizarTarefaProjetoResponse>(setDataFimResult.Error);

        var tarefaAtualizada = await tarefaRepository.AtualizarTarefaAsync(tarefa, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(tarefaAtualizada.ToAtualizarTarefaProjetoResponse());
    }
}
