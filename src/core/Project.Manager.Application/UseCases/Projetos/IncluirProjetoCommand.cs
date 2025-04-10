using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Projetos;

public sealed record IncluirProjetoCommand(Guid UsuarioId, string Nome, string Descricao, DateTime DataInicio, DateTime DataFim) : ICommand;
