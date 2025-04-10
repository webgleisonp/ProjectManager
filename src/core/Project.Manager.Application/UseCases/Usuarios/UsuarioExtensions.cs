using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.UseCases.Usuarios;

public static class UsuarioExtensions
{
    public static IncluirUsuarioCommandResponse ToResponse(this Usuario usuario)
    {
        return new IncluirUsuarioCommandResponse(
            usuario.Id.Id,
            usuario.Nome,
            usuario.Email.Value);
    }
}