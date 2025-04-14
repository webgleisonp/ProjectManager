using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Infra.Data.Repositories;

internal sealed class HistoricoRepository(SqlServerDbContext sqlServerDbContext) : IHistoricoRepository
{
    public async ValueTask<Historico> AdicionarHistoricoAsync(Historico historico, CancellationToken cancellationToken)
    {
        await sqlServerDbContext.Historico.AddAsync(historico, cancellationToken);

        return historico;
    }
}
