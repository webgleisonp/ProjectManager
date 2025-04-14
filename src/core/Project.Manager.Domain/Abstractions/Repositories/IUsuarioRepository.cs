using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Abstractions.Repositories;

public interface IUsuarioRepository
{
    ValueTask<Usuario> AdicionarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken);
    ValueTask<Usuario> RetornarUsuarioAsync(UsuarioId usuarioId, CancellationToken cancellationToken);
}
