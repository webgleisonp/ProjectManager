using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record RetornarTarefasProjetoResponse(Guid Id, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim, StatusTarefa Status, PrioridadeTarefa Prioridade);
