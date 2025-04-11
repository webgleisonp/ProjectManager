using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record IncluirTarefaProjetoCommand(Guid ProjetoId, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim) : ICommand;