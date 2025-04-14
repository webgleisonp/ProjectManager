using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Entities;

public class Historico
{
    private Historico() { }

    private Historico(HistoricoId id, TarefaId tarefaId, UsuarioId usuarioId, TipoHistorico tipo, string descricao, DateTime dataCriacao)
    {
        Id = id;
        TarefaId = tarefaId;
        UsuarioId = usuarioId;
        Tipo = tipo;
        Descricao = descricao;
        DataCriacao = dataCriacao;
    }

    public HistoricoId Id { get; init; } = null!;
    public TarefaId TarefaId { get; init; } = null!;
    public UsuarioId UsuarioId { get; init; } = null!;
    public TipoHistorico Tipo { get; init; }
    public string Descricao { get; init; } = null!;
    public DateTime DataCriacao { get; init; }

    public virtual Tarefa Tarefa { get; private set; } = null!;

    public virtual Usuario Usuario { get; private set; } = null!;

    public static Result<Historico> CriarAtualizacao(HistoricoId id, TarefaId tarefaId, UsuarioId usuarioId, string descricao)
    {
        if (id.Value == Guid.Empty)
            return Result.Failure<Historico>(HistoricoErrors.IdHistoricoNaoPodeSerVazio);

        if (tarefaId.Value == Guid.Empty)
            return Result.Failure<Historico>(TarefaErrors.IdTarefaNaoPodeSerVazio);

        if (usuarioId.Value == Guid.Empty)
            return Result.Failure<Historico>(UsuarioErrors.IdUsuarioNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Historico>(HistoricoErrors.DescricaoHistoricoNaoPodeSerVazia);

        return new Historico(id, tarefaId, usuarioId, TipoHistorico.Atualizacao, descricao, DateTime.UtcNow)!;
    }

    public static Result<Historico> CriarComentario(HistoricoId id, TarefaId tarefaId, UsuarioId usuarioId, string descricao)
    {
        if (id.Value == Guid.Empty)
            return Result.Failure<Historico>(HistoricoErrors.IdHistoricoNaoPodeSerVazio);

        if (tarefaId.Value == Guid.Empty)
            return Result.Failure<Historico>(TarefaErrors.IdTarefaNaoPodeSerVazio);

        if (usuarioId.Value == Guid.Empty)
            return Result.Failure<Historico>(UsuarioErrors.IdUsuarioNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Historico>(HistoricoErrors.DescricaoHistoricoNaoPodeSerVazia);

        return new Historico(id, tarefaId, usuarioId, TipoHistorico.Comentario, descricao, DateTime.UtcNow)!;
    }

    public Result SetTarefa(Tarefa tarefa)
    {
        if (tarefa is null)
            return Result.Failure(TarefaErrors.TarefaNaoPodeSerNula);

        Tarefa = tarefa;

        return Result.Success();
    }

    public Result SetUsuario(Usuario usuario)
    {
        if (usuario is null)
            return Result.Failure(UsuarioErrors.UsuarioNaoPodeSerNulo);

        Usuario = usuario;

        return Result.Success();
    }
}
