using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Shared;

namespace Project.Manager.Application.UseCases.Usuarios;

internal sealed class IncluirUsuarioCommandHandler(IUsuarioRepository usuarioRepository, IUnityOfWork unityOfWork) : ICommandHandler<IncluirUsuarioCommand, IncluirUsuarioCommandResponse>
{
    public async ValueTask<Result<IncluirUsuarioCommandResponse>> HandleAsync(IncluirUsuarioCommand command, CancellationToken cancellationToken = default)
    {
        var id =Guid.NewGuid();

        var novoUsuarioResult = Usuario.Criar(id.ToUsuarioId(),
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