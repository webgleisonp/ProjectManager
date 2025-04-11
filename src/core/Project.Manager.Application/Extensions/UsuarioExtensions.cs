using Project.Manager.Application.UseCases.Usuarios;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.Extensions;

public static class UsuarioExtensions
{
    public static IncluirUsuarioCommandResponse ToResponse(this Usuario usuario)
    {
        return new IncluirUsuarioCommandResponse(
            usuario.Id.Value,
            usuario.Nome,
            usuario.Email.Value);
    }
}