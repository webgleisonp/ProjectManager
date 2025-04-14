using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record AtualizarTarefaProjetoResponse(Guid TarefaId, string Projeto, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim, StatusTarefa Status, PrioridadeTarefa Prioridade);
