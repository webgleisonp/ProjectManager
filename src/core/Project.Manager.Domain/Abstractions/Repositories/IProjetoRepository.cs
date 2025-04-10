using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Abstractions.Repositories;

public interface IProjetoRepository
{
    ValueTask<Projeto> AdicionarProjetoAsync(Projeto projeto, CancellationToken cancellationToken);
    ValueTask<IEnumerable<Projeto>> RetornarProjetosUsuarioAsync(UsuarioId usuarioId, CancellationToken cancellationToken);
}
