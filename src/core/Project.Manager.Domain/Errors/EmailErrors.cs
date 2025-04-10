namespace Project.Manager.Domain.Errors;

public static class EmailErrors
{
    public static Error EmailNaoPodeSerVazio => new("Email.NaoPodeSerVazio", "O campo Email não pode ser vazio.");

    public static Error EmailInvalido => new("Email.Invalido", "Email inválido.");
}