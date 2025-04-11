using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Abstractions.Repositories;

public interface ITarefaRepository
{
    ValueTask<Tarefa> AdicionarTarefaAsync(Tarefa value, CancellationToken cancellationToken);
    ValueTask<IEnumerable<Tarefa>> RetornaTarefasPorProjetoAsync(ProjetoId projetoId, CancellationToken cancellationToken);
}
