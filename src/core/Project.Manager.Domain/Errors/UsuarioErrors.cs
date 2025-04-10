namespace Project.Manager.Domain.Errors;

public static class UsuarioErrors
{
    public static Error NomeUsuarioNaoPodeSerVazio => new("Usuario.NaoPodeSerVazio", "O campo Nome do Usuário não pode ser vazio.");
}