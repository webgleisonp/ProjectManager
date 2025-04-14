using Project.Manager.Application.UseCases.Comentarios;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.Extensions;

public static class HistoricoExtensions
{
    public static IncluirComentarioTarefaResponse ToIncluirComentarioTarefaResponse(this Historico historico)
    {
        return new IncluirComentarioTarefaResponse(historico.Id.Value,
            historico.Tarefa.Nome,
            historico.Usuario.Nome,
            historico.Descricao);
    }
}
