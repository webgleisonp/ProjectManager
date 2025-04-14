using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;

namespace Project.Manager.Application.UseCases.Comentarios;

internal sealed class IncluirComentarioTarefaCommandHandler(ITarefaRepository tarefaRepository,
    IUsuarioRepository usuarioRepository,
    IHistoricoRepository historicoRepository,
    IUnityOfWork unityOfWork) : ICommandHandler<IncluirComentarioTarefaCommand, IncluirComentarioTarefaResponse>
{
    public async ValueTask<Result<IncluirComentarioTarefaResponse>> HandleAsync(IncluirComentarioTarefaCommand command, CancellationToken cancellationToken = default)
    {
        var tarefa = await tarefaRepository.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), cancellationToken);

        if (tarefa is null)
            return Result.Failure<IncluirComentarioTarefaResponse>(TarefaErrors.TarefaNaoEncontrada);

        var usuario = await usuarioRepository.RetornarUsuarioAsync(command.UsuarioId.ToUsuarioId(), cancellationToken);

        if (usuario is null)
            return Result.Failure<IncluirComentarioTarefaResponse>(UsuarioErrors.UsuarioNaoEncontrado);

        var id = Guid.NewGuid();

        var comentario = Historico.CriarComentario(id.ToHistorioId(), tarefa.Id, command.UsuarioId.ToUsuarioId(), command.Comentario);

        if (comentario.IsFailure)
            return Result.Failure<IncluirComentarioTarefaResponse>(comentario.Error);

        var comentarioIncluido = await historicoRepository.AdicionarHistoricoAsync(comentario.Value, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(comentarioIncluido.ToIncluirComentarioTarefaResponse());
    }
}