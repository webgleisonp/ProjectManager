using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.UseCases.Projetos;

internal sealed class IncluirProjetoCommandHandler(IProjetoRepository projetoRepository, IUnityOfWork unityOfWork) : ICommandHandler<IncluirProjetoCommand, IncluirProjetoCommandResponse>
{
    public async ValueTask<Result<IncluirProjetoCommandResponse>> HandleAsync(IncluirProjetoCommand command, CancellationToken cancellationToken = default)
    {
        var novoProjetoResult = Projeto.Criar(new ProjetoId(Guid.NewGuid()),
            new UsuarioId(command.UsuarioId),
            command.Nome,
            command.Descricao,
            command.DataInicio,
            command.DataFim);

        if(novoProjetoResult.IsFailure)
            return Result.Failure<IncluirProjetoCommandResponse>(novoProjetoResult.Error);

        var novoProjeto = await projetoRepository.AdicionarProjetoAsync(novoProjetoResult.Value, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(novoProjeto.ToIncluirProjetoCommandResponse());
    }
}
