namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record IncluirTarefaProjetoResponse(Guid Id, string Projeto, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim);
