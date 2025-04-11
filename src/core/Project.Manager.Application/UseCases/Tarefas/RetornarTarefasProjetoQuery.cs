using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record RetornarTarefasProjetoQuery(Guid ProjetoId) : IQuery;