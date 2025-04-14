using Microsoft.EntityFrameworkCore;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Repositories;

internal sealed class UsuarioRepository(SqlServerDbContext sqlServerDbContext) : IUsuarioRepository
{
    public async ValueTask<Usuario> AdicionarUsuarioAsync(Usuario usuario, CancellationToken cancellationToken)
    {
        await sqlServerDbContext.Usuarios.AddAsync(usuario, cancellationToken);

        return usuario;
    }

    public async ValueTask<Usuario> RetornarUsuarioAsync(UsuarioId usuarioId, CancellationToken cancellationToken)
    {
        var data = await sqlServerDbContext.Usuarios
            .FirstOrDefaultAsync(x => x.Id == usuarioId, cancellationToken);

        return data!;
    }
}
