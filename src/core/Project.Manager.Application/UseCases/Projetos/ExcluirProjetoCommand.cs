using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Projetos;

public sealed record ExcluirProjetoCommand(Guid Id) : ICommand;