using NSubstitute;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class IncluirProjetoCommandHandlerTests
{
    private readonly IncluirProjetoCommandHandler _incluirProjetoCommandHandler;
    private readonly IUnityOfWork _unityOfWorkMock;
    private readonly IProjetoRepository _projetoRepositoryMock;

    public IncluirProjetoCommandHandlerTests()
    {
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();
        _projetoRepositoryMock = Substitute.For<IProjetoRepository>();

        _incluirProjetoCommandHandler = new IncluirProjetoCommandHandler(_projetoRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Incluir_Projeto_Com_Sucesso()
    {
        // Arrange
        var command = new IncluirProjetoCommand(Guid.NewGuid(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));
        var projeto = Projeto.Criar(new ProjetoId(Guid.NewGuid()), new UsuarioId(command.UsuarioId), command.Nome, command.Descricao, command.DataInicio, command.DataFim).Value;
        _projetoRepositoryMock.AdicionarProjetoAsync(Arg.Any<Projeto>(), Arg.Any<CancellationToken>())
            .Returns(projeto);

        // Act
        var result = await _incluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(projeto.Nome, result.Value.Nome);
        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Projeto_Invalido()
    {
        // Arrange
        var command = new IncluirProjetoCommand(Guid.NewGuid(), "", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));

        // Act
        var result = await _incluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        await _projetoRepositoryMock.DidNotReceive().AdicionarProjetoAsync(Arg.Any<Projeto>(), Arg.Any<CancellationToken>());
        await _unityOfWorkMock.DidNotReceive().SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Der_Erro_Ao_Adicionar_Projeto()
    {
        // Arrange
        var command = new IncluirProjetoCommand(Guid.NewGuid(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));
        var projeto = Projeto.Criar(new ProjetoId(Guid.NewGuid()), new UsuarioId(command.UsuarioId), command.Nome, command.Descricao, command.DataInicio, command.DataFim).Value;
        _projetoRepositoryMock.AdicionarProjetoAsync(Arg.Any<Projeto>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Projeto>>(_ => throw new Exception("Erro ao adicionar projeto"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirProjetoCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Der_Erro_No_SaveChanges()
    {
        // Arrange
        var command = new IncluirProjetoCommand(Guid.NewGuid(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));
        var projeto = Projeto.Criar(new ProjetoId(Guid.NewGuid()), new UsuarioId(command.UsuarioId), command.Nome, command.Descricao, command.DataInicio, command.DataFim).Value;
        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao salvar no banco"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirProjetoCommandHandler.HandleAsync(command));
    }
}
