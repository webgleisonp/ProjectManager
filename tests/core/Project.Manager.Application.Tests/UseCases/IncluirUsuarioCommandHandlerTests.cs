using NSubstitute;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
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
        var id = Guid.NewGuid();

        var command = new IncluirUsuarioCommand("João", "joao@email.com", "Senha123", "Senha123");

        var novoUsuarioCriado = Usuario.Criar(id.ToUsuarioId(), command.Nome, command.Email, command.Senha, command.ConfirmarSenha).Value;

        _usuarioRepositoryMock.AdicionarUsuarioAsync(Arg.Is<Usuario>(u => u.Nome == novoUsuarioCriado.Nome), Arg.Any<CancellationToken>())
            .Returns(novoUsuarioCriado);

        // Act
        var result = await _incluirUsuarioCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(novoUsuarioCriado.Nome, result.Value.Nome);

        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Deve_Retornar_Falha_Quando_Instancia_Usuario_For_Invalida()
    {
        // Arrange
        var command = new IncluirUsuarioCommand("", "email", "senha", "confirmarSenha");

        // Act
        var result = await _incluirUsuarioCommandHandler.HandleAsync(command);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Adicionar_Usuario_Via_Repositorio()
    {
        // Arrange
        var command = new IncluirUsuarioCommand("João", "joao@email.com", "Senha123", "Senha123");

        _usuarioRepositoryMock.AdicionarUsuarioAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Usuario>>(_ => throw new Exception("Falha no repositório"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirUsuarioCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new IncluirUsuarioCommand("João", "joao@email.com", "Senha123", "Senha123");

        var novoUsuarioCriado = Usuario.Criar(id.ToUsuarioId(), command.Nome, command.Email, command.Senha, command.ConfirmarSenha).Value;

        _usuarioRepositoryMock.AdicionarUsuarioAsync(Arg.Is<Usuario>(u => u.Nome == novoUsuarioCriado.Nome), Arg.Any<CancellationToken>())
            .Returns(novoUsuarioCriado);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao salvar no banco"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirUsuarioCommandHandler.HandleAsync(command));
    }
}