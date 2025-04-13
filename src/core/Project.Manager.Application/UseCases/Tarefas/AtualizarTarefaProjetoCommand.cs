using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record AtualizarTarefaProjetoCommand(Guid TarefaId, Guid ProjetoId, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim, StatusTarefa Status) : ICommand;
