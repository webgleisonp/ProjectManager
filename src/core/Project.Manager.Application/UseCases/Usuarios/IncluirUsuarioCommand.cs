using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Usuarios;

public sealed record IncluirUsuarioCommand(
    string Nome,
    string Email,
    string Senha,
    string ConfirmarSenha) : ICommand;
