namespace Project.Manager.Application.UseCases.Usuarios;

public sealed record IncluirUsuarioCommandResponse(
    Guid Id,
    string Nome,
    string Email);
