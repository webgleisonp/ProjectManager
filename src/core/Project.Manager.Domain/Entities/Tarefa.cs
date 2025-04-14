using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Entities;

public class Tarefa
{
    private Tarefa() { }

    private Tarefa(TarefaId id, ProjetoId projetoId, UsuarioId usuarioId, string nome, string descricao, DateTime dataInicio, DateTime dataFim, StatusTarefa status, PrioridadeTarefa prioridade)
    {
        Id = id;
        ProjetoId = projetoId;
        UsuarioId = usuarioId;
        Nome = nome;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataFim = dataFim;
        Status = status;
        Prioridade = prioridade;
    }

    public TarefaId Id { get; init; } = null!;
    public ProjetoId ProjetoId { get; private set; } = null!;
    public UsuarioId UsuarioId { get; private set; } = null!;
    public string Nome { get; private set; } = null!;
    public string Descricao { get; private set; } = null!;
    public DateTime DataInicio { get; private set; }
    public DateTime DataFim { get; private set; }
    public StatusTarefa Status { get; private set; }
    public PrioridadeTarefa Prioridade { get; private set; }

    public virtual Projeto Projeto { get; private set; } = null!;

    public virtual Usuario Usuario { get; private set; } = null!;

    public virtual ICollection<Historico> HistoricoEventos { get; } = [];

    public static Result<Tarefa> Criar(TarefaId id, ProjetoId projetoId, UsuarioId usuarioId, string nome, string descricao, DateTime dataInicio, DateTime dataFim, StatusTarefa status, PrioridadeTarefa prioridade)
    {
        if (id.Value == Guid.Empty)
            return Result.Failure<Tarefa>(TarefaErrors.IdTarefaNaoPodeSerVazio);

        if (projetoId.Value == Guid.Empty)
            return Result.Failure<Tarefa>(ProjetoErrors.IdProjetoNaoPodeSerVazio);

        if (usuarioId.Value == Guid.Empty)
            return Result.Failure<Tarefa>(UsuarioErrors.IdUsuarioNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Tarefa>(TarefaErrors.NomeTarefaNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Tarefa>(TarefaErrors.DescricaoTarefaNaoPodeSerVazia);

        if (dataInicio >= dataFim)
            return Result.Failure<Tarefa>(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

        return new Tarefa(id, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade)!;
    }

    public Result SetNome(string nome)
    {
        if (Nome != nome && !string.IsNullOrWhiteSpace(nome))
        {
            Nome = nome;

            var historico = Historico.CriarAtualizacao(new HistoricoId(Guid.NewGuid()), Id, UsuarioId, $"Nome alterado de '{Nome}' para '{nome}'");

            if (historico.IsFailure)
                return Result.Failure(historico.Error);

            HistoricoEventos.Add(historico.Value);
        }

        return Result.Success();
    }

    public Result SetDescricao(string descricao)
    {
        if (Descricao != descricao && !string.IsNullOrWhiteSpace(descricao))
        {
            Descricao = descricao;

            var historico = Historico.CriarAtualizacao(new HistoricoId(Guid.NewGuid()), Id, UsuarioId, $"Descrição alterada de '{Descricao}' para '{descricao}'");

            if (historico.IsFailure)
                return Result.Failure(historico.Error);
        }

        return Result.Success();
    }

    public Result SetDataInicio(DateTime dataInicio)
    {
        if (DataInicio != dataInicio && dataInicio != DateTime.MinValue)
        {
            if (dataInicio >= DataFim)
                return Result.Failure(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

            DataInicio = dataInicio;

            var historico = Historico.CriarAtualizacao(new HistoricoId(Guid.NewGuid()), Id, UsuarioId, $"Data Inicio alterada de '{DataInicio}' para '{dataInicio}'");

            if (historico.IsFailure)
                return Result.Failure(historico.Error);
        }

        return Result.Success();
    }

    public Result SetDataFim(DateTime dataFim)
    {
        if (DataFim != dataFim && dataFim != DateTime.MinValue)
        {
            if (DataInicio >= dataFim)
                return Result.Failure(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

            DataFim = dataFim;

            var historico = Historico.CriarAtualizacao(new HistoricoId(Guid.NewGuid()), Id, UsuarioId, $"Data Fim alterada de '{DataFim}' para '{dataFim}'");

            if (historico.IsFailure)
                return Result.Failure(historico.Error);
        }

        return Result.Success();
    }

    public Result SetStatus(StatusTarefa status)
    {
        if (Status != status && Enum.IsDefined(typeof(StatusTarefa), status))
        {
            Status = status;

            var statusAtual = Enum.GetName(typeof(StatusTarefa), Status);
            var novoStatus = Enum.GetName(typeof(StatusTarefa), status);

            var historico = Historico.CriarAtualizacao(new HistoricoId(Guid.NewGuid()), Id, UsuarioId, $"Status alterado de '{statusAtual}' para '{novoStatus}'");

            if (historico.IsFailure)
                return Result.Failure(historico.Error);
        }

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