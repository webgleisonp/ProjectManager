namespace Project.Manager.Application.UseCases.Comentarios;

public sealed record IncluirComentarioTarefaResponse(Guid Id, string Tarefa, string Usuario, string Comentario);