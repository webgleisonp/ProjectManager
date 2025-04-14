namespace Project.Manager.Application.UseCases.Usuarios;

public sealed record IncluirUsuarioCommandRequest(string Nome,
    string Email,
    string Senha,
    string ConfirmarSenha);
