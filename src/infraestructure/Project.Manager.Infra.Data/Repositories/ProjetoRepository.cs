using Microsoft.EntityFrameworkCore;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Repositories;

internal sealed class ProjetoRepository(SqlServerDbContext sqlServerDbContext) : IProjetoRepository
{
    public async ValueTask<Projeto> AdicionarProjetoAsync(Projeto projeto, CancellationToken cancellationToken)
    {
        await sqlServerDbContext.Projetos.AddAsync(projeto, cancellationToken);

        return projeto;
    }

    public async ValueTask RemoverProjetoAsync(Projeto projeto, CancellationToken cancellationToken)
    {
        await ValueTask.FromResult(sqlServerDbContext.Remove(projeto));
    }

    public async ValueTask<Projeto> RetornarProjetoAsync(ProjetoId projetoId, CancellationToken cancellationToken)
    {
        var data = await sqlServerDbContext.Projetos
            .Include(nameof(Usuario))
            .AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == projetoId, cancellationToken);

        return data!;
    }

    public async ValueTask<IEnumerable<Projeto>> RetornarProjetosUsuarioAsync(UsuarioId usuarioId, CancellationToken cancellationToken)
    {
        var data = await sqlServerDbContext.Projetos
            .Where(p => p.UsuarioId == usuarioId)
            .Include(nameof(Usuario))
            .AsSplitQuery()
            .ToListAsync(cancellationToken);

        return data!;
    }
}
