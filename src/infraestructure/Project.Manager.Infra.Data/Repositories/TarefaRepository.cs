using Microsoft.EntityFrameworkCore;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Repositories;

internal sealed class TarefaRepository(SqlServerDbContext sqlServerDbContext) : ITarefaRepository
{
    public async ValueTask<Tarefa> AdicionarTarefaAsync(Tarefa tarefa, CancellationToken cancellationToken)
    {
        await sqlServerDbContext.Tarefas.AddAsync(tarefa, cancellationToken);

        return tarefa;
    }

    public async ValueTask<Tarefa> AtualizarTarefaAsync(Tarefa tarefa, CancellationToken cancellationToken)
    {
        await ValueTask.FromResult(sqlServerDbContext.Tarefas.Update(tarefa));

        return tarefa;
    }

    public async ValueTask RemoverTarefaProjetoAsync(Tarefa tarefa, CancellationToken cancellationToken)
    {
        await ValueTask.FromResult(sqlServerDbContext.Tarefas.Remove(tarefa));
    }

    public async ValueTask<Tarefa> RetornarTarefaAsync(TarefaId tarefaId, CancellationToken cancellationToken)
    {
        var data = await sqlServerDbContext.Tarefas
            .Include(nameof(Projeto))
            .AsSplitQuery()
            .FirstOrDefaultAsync(t => t.Id == tarefaId, cancellationToken: cancellationToken);

        return data!;
    }

    public async ValueTask<IEnumerable<Tarefa>> RetornaTarefasPorProjetoAsync(ProjetoId projetoId, CancellationToken cancellationToken)
    {
        var data = await sqlServerDbContext.Tarefas
            .Where(t => t.ProjetoId == projetoId)
            .Include(nameof(Projeto))
            .ToListAsync(cancellationToken);

        return data!;
    }
}
