namespace Project.Manager.Application.UseCases.Comentarios;

public sealed record IncluirComentarioTarefaRequest(Guid UsuarioId,
    string Comentario);
