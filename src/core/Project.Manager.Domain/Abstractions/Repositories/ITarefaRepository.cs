using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Abstractions.Repositories;

public interface ITarefaRepository
{
    ValueTask<Tarefa> AdicionarTarefaAsync(Tarefa tarefa, CancellationToken cancellationToken);
    ValueTask<Tarefa> AtualizarTarefaAsync(Tarefa tarefa, CancellationToken cancellationToken);
    ValueTask RemoverTarefaProjetoAsync(Tarefa tarefa, CancellationToken cancellationToken);
    ValueTask<Tarefa> RetornarTarefaAsync(TarefaId tarefaId, CancellationToken cancellationToken);
    ValueTask<IEnumerable<Tarefa>> RetornaTarefasPorProjetoAsync(ProjetoId projetoId, CancellationToken cancellationToken);
}
