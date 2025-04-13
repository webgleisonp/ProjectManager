using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Entities;
public class Usuario : AgregateRoot<UsuarioId>
{
    private Usuario(UsuarioId id, string nome, Email email, Senha senha, bool ativo)
        : base(id)
    {
        Nome = nome;
        Email = email;
        Senha = senha;
        Ativo = ativo;
    }

    public string Nome { get; init; } = null!;
    public Email Email { get; init; } = null!;
    public Senha Senha { get; init; } = null!;
    public bool Ativo { get; init; }

    public virtual ICollection<Projeto> Projetos { get; } = [];

    public static Result<Usuario> Criar(UsuarioId id, string nome, string email, string senha, string confirmarSenha)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Usuario>(UsuarioErrors.NomeUsuarioNaoPodeSerVazio);

        var emailResult = Email.Criar(email);

        if(emailResult.IsFailure)
            return Result.Failure<Usuario>(emailResult.Error);

        var senhaResult = Senha.Criar(senha, confirmarSenha);

        if (senhaResult.IsFailure)
            return Result.Failure<Usuario>(senhaResult.Error);

        return new Usuario(id, nome, emailResult.Value, senhaResult.Value, true)!;
    }
}
