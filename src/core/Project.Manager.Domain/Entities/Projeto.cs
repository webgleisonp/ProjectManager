using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Entities;

public class Projeto
{
    private Projeto(ProjetoId id, UsuarioId usuarioId, string nome, string descricao, DateTime dataInicio, DateTime dataFim, bool ativo)
    {
        Id = id;
        UsuarioId = usuarioId;
        Nome = nome;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Ativo = ativo;
    }

    public ProjetoId Id { get; init; } = null!;
    public UsuarioId UsuarioId { get; init; } = null!;
    public string Nome { get; init; } = null!;
    public string Descricao { get; init; } = null!;
    public DateTime DataInicio { get; init; }
    public DateTime DataFim { get; init; }
    public bool Ativo { get; init; }

    public virtual Usuario Usuario { get; private set; } = null!;

    public static Result<Projeto> Criar(ProjetoId id, UsuarioId usuarioId, string nome, string descricao, DateTime dataInicio, DateTime dataFim)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Projeto>(ProjetoErrors.NomeProjetoNaoPodeSerVazio);

        if (string.IsNullOrEmpty(descricao))
            return Result.Failure<Projeto>(ProjetoErrors.DescricaoProjetoNaoPodeSerVazia);

        if (dataInicio >= dataFim)
            return Result.Failure<Projeto>(ProjetoErrors.DataInicioDeveSerMenorQueDataFim);

        return new Projeto(id, usuarioId, nome, descricao, dataInicio, dataFim, true)!;
    }

    public void SetUsuario(Usuario usuario)
    {
        Usuario = usuario;
    }
}