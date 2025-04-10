using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Entities;

public sealed class Tarefa
{
    private Tarefa(TarefaId id, string nome, string descricao, DateTime dataInicio, DateTime dataFim)
    {
        Id = id;
        Nome = nome;
        Descricao = descricao;
        DataInicio = dataInicio;
        DataFim = dataFim;
    }

    public TarefaId Id { get; init; } = null!;
    public string Nome { get; init; } = null!;
    public string Descricao { get; init; } = null!;
    public DateTime DataInicio { get; init; }
    public DateTime DataFim { get; init; }

    public static Result<Tarefa> Criar(TarefaId id, string nome, string descricao, DateTime dataInicio, DateTime dataFim)
    {
        if (string.IsNullOrWhiteSpace(nome))
            return Result.Failure<Tarefa>(TarefaErrors.NomeTarefaNaoPodeSerVazio);

        if (string.IsNullOrWhiteSpace(descricao))
            return Result.Failure<Tarefa>(TarefaErrors.DescricaoTarefaNaoPodeSerVazia);

        if (dataInicio > dataFim)
            return Result.Failure<Tarefa>(TarefaErrors.DataInicioDeveSerMenorQueDataFim);

        return new Tarefa(id, nome, descricao, dataInicio, dataFim)!;
    }
}