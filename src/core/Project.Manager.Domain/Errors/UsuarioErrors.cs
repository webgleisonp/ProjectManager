namespace Project.Manager.Domain.Errors;

public static class UsuarioErrors
{
    public static readonly Error IdUsuarioNaoPodeSerVazio = new("Usuario.IdUsuarioNaoPodeSerVazio", "O id do usuário não pode ser vazio.");
    public static readonly Error NomeUsuarioNaoPodeSerVazio = new("Usuario.NaoPodeSerVazio", "O campo Nome do Usuário não pode ser vazio.");
    public static readonly Error UsuarioNaoPodeSerNulo = new("Usuario.UsuarioNaoPodeSerNulo", "Usuario não pode ser nulo.");
}