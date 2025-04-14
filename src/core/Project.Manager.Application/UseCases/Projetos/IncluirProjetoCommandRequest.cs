namespace Project.Manager.Application.UseCases.Projetos;

public sealed record IncluirProjetoCommandRequest(Guid UsuarioId,
    string Nome,
    string Descricao,
    DateOnly DataInicio,
    DateOnly DataFim);
