using Project.Manager.Domain.Entities;

namespace Project.Manager.Domain.Abstractions.Repositories;

public interface IHistoricoRepository
{
    ValueTask<Historico> AdicionarHistoricoAsync(Historico historico, CancellationToken cancellationToken);
}
