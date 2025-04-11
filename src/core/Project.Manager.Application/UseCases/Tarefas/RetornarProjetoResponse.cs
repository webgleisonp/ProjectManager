namespace Project.Manager.Application.UseCases.Tarefas;

public sealed record RetornarProjetoResponse(Guid Id, string Nome, string Descricao, DateTime DataInicio, DateTime? DataTermino, string UsuarioNome, IEnumerable<RetornarTarefasProjetoResponse> Tarefas);
