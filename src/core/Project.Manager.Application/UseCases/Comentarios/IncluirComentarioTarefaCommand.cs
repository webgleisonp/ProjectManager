using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Comentarios;

public sealed record IncluirComentarioTarefaCommand(Guid TarefaId, Guid UsuarioId, string Comentario) : ICommand;
