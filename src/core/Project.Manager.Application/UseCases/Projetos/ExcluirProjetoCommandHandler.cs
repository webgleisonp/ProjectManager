using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Projetos;

internal sealed class ExcluirProjetoCommandHandler(IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository, IUnityOfWork unityOfWork) : ICommandHandler<ExcluirProjetoCommand>
{
    public async ValueTask<Result> HandleAsync(ExcluirProjetoCommand command, CancellationToken cancellationToken = default)
    {
        var projeto = await projetoRepository.RetornarProjetoAsync(command.Id.ToProjetoId(), cancellationToken);

        if (projeto is null)
            return Result.Failure(ProjetoErrors.ProjetoNaoEncontrado);

        var tarefas = await tarefaRepository.RetornaTarefasPorProjetoAsync(projeto.Id, cancellationToken);

        if (tarefas.Any(t => t.Status != StatusTarefa.Concluida))
            return Result.Failure(ProjetoErrors.TarefasPendentesConclusao);

        await projetoRepository.RemoverProjetoAsync(projeto, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}