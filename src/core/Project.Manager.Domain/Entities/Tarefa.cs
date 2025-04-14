using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Entities;

public class Tarefa
{
    private Tarefa() { }

    private Tarefa(TarefaId id, ProjetoId projetoId, string nome, string descricao, DateTime dataInicio, DateTime dataFim, StatusTarefa status, PrioridadeTarefa prioridade)
    {
        Id = id;
        ProjetoId = projetoId;
        Nome = nome;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Status = status;
        Prioridade = prioridade;
    }

    public TarefaId Id { get; init; } = null!;
    public ProjetoId ProjetoId { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public StatusTarefa Status { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }

    public virtual Projeto Projeto { get; private set; } = null!;

    public virtual ICollection<Historico> Historico { get; } = [];

    public static Result<Tarefa> Criar(TarefaId id, ProjetoId projetoId, string nome, string descricao, DateTime dataInicio, DateTime dataFim, StatusTarefa status, PrioridadeTarefa prioridade)
    {
        if(id.Value == Guid.Empty)
            return Result.Failure<Tarefa>(TarefaErrors.IdTarefaNaoPodeSerVazio);

        if (projetoId.Value == Guid.Empty)
            return Result.Failure<Tarefa>(ProjetoErrors.IdProjetoNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Tarefa>(TarefaErrors.NomeTarefaNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Tarefa>(TarefaErrors.DescricaoTarefaNaoPodeSerVazia);

        if (dataInicio >= dataFim)
            return Result.Failure<Tarefa>(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

        return new Tarefa(id, projetoId, nome, descricao, dataInicio, dataFim, status, prioridade)!;
    }

    public Result SetNome(string nome)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure(TarefaErrors.NomeTarefaNaoPodeSerVazio);

        Nome = nome;

        return Result.Success();
    }

    public Result SetDescricao(string descricao)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure(TarefaErrors.DescricaoTarefaNaoPodeSerVazia);

        Descricao = descricao;

        return Result.Success();
    }

    public Result SetDataInicio(DateTime dataInicio)
    {
        if (dataInicio >= DataFim)
            return Result.Failure(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

        DataInicio = dataInicio;

        return Result.Success();
    }

    public Result SetDataFim(DateTime dataFim)
    {
        if (DataInicio >= dataFim)
            return Result.Failure(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

        DataFim = dataFim;

        return Result.Success();
    }

    public Result SetStatus(StatusTarefa status)
    {
        Status = status;

        return Result.Success();
    }

    public Result SetPrioridade(PrioridadeTarefa prioridade)
    {
        Prioridade = prioridade;

        return Result.Success();
    }

    public Result SetProjeto(Projeto projeto)
    {
        if (projeto is null)
            return Result.Failure(ProjetoErrors.ProjetoNaoPodeSerNulo);

        Projeto = projeto;

        return Result.Success();
    }
}