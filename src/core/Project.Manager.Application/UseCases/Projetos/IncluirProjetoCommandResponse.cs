namespace Project.Manager.Application.UseCases.Projetos;

public sealed record IncluirProjetoCommandResponse(Guid Id, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim);
