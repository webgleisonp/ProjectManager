using NSubstitute;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.UseCases.Usuarios;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class IncluirUsuarioCommandHandlerTests
{
    private readonly IncluirUsuarioCommandHandler _incluirUsuarioCommandHandler;
    private readonly IUnityOfWork _unityOfWorkMock;
    private readonly IUsuarioRepository _usuarioRepositoryMock;

    public IncluirUsuarioCommandHandlerTests()
    {
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();

        _incluirUsuarioCommandHandler = new IncluirUsuarioCommandHandler(_usuarioRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Incluir_Usuario_Com_Sucesso()
    {
        // Arrange
        var command = new IncluirUsuarioCommand("João", "joao@email.com", "Senha123", "Senha123");

        var usuario = Usuario.Criar(new UsuarioId(Guid.NewGuid()), command.Nome, command.Email, command.Senha, command.ConfirmarSenha).Value;

        _usuarioRepositoryMock.AdicionarUsuarioAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(usuario);

        // Act
        var result = await _incluirUsuarioCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(usuario.Nome, result.Value.Nome);
        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Deve_Retornar_Falha_Quando_Usuario_Invalido()
    {
        // Arrange
        var command = new IncluirUsuarioCommand("João", "email-invalido", "123", "123");

        // Act
        var result = await _incluirUsuarioCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        await _usuarioRepositoryMock.DidNotReceive().AdicionarUsuarioAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
        await _unityOfWorkMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Der_Erro_Ao_Adicionar_Usuario()
    {
        // Arrange
        var command = new IncluirUsuarioCommand("João", "joao@email.com", "Senha123", "Senha123");

        var usuario = Usuario.Criar(new UsuarioId(Guid.NewGuid()), command.Nome, command.Email, command.Senha, command.ConfirmarSenha).Value;

        _usuarioRepositoryMock.AdicionarUsuarioAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Usuario>>(_ => throw new Exception("Falha no repositório"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirUsuarioCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Der_Erro_No_SaveChanges()
    {
        // Arrange
        var command = new IncluirUsuarioCommand("João", "joao@email.com", "Senha123", "Senha123");

        var usuario = Usuario.Criar(new UsuarioId(Guid.NewGuid()), command.Nome, command.Email, command.Senha, command.ConfirmarSenha).Value;

        _usuarioRepositoryMock.AdicionarUsuarioAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao salvar no banco"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirUsuarioCommandHandler.HandleAsync(command));
    }
}