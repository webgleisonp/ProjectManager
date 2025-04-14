using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record IncluirTarefaProjetoRequest(
    Guid ProjetoId,
    string Nome,
    string Descricao,
    DateOnly DataInicio,
    DateOnly DataFim,
    PrioridadeTarefa Prioridade);
