using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.UseCases.Usuarios;

internal sealed class IncluirUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IUnityOfWork unityOfWork) : ICommandHandler<IncluirUsuarioCommand, IncluirUsuarioCommandResponse>
{
    public async ValueTask<Result<IncluirUsuarioCommandResponse>> HandleAsync(IncluirUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var novoUsuarioResult = Usuario.Criar(new UsuarioId(Guid.NewGuid()),
            command.Nome,
            command.Email,
            command.Senha,
            command.ConfirmarSenha);

        if(novoUsuarioResult.IsFailure)
            return Result.Failure<IncluirUsuarioCommandResponse>(novoUsuarioResult.Error);

        var novoUsuario = await usuarioRepository.AdicionarUsuarioAsync(novoUsuarioResult.Value, cancellationToken);

        await unityOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(novoUsuario.ToResponse());
    }
}