using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Infra.Data.Repositories;

internal sealed class UsuarioRepository(SqlServerDbContext sqlServerDbContext) : IUsuarioRepository
{
    public async ValueTask<Usuario> AdicionarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        await sqlServerDbContext.Usuarios.AddAsync(usuario, cancellationToken);
        
        return usuario;
    }
}
