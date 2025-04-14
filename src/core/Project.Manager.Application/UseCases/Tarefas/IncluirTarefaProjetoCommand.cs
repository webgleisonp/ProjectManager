using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.ValueObjects.Enums;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record IncluirTarefaProjetoCommand(Guid ProjetoId, Guid UsuarioId, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim, PrioridadeTarefa Prioridade) : ICommand;