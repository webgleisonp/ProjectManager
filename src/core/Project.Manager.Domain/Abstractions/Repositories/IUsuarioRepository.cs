using Project.Manager.Domain.Entities;

namespace Project.Manager.Domain.Abstractions.Repositories;

public interface IUsuarioRepository
{
    ValueTask<Usuario> AdicionarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken);
}
