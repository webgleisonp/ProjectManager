using NSubstitute;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;

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
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new IncluirProjetoCommand(usuarioId, "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));

        var novoProjetoCriado = Projeto.Criar(id.ToProjetoId(), command.UsuarioId.ToUsuarioId(), command.Nome, command.Descricao, command.DataInicio, command.DataFim).Value;

        _projetoRepositoryMock.AdicionarProjetoAsync(Arg.Is<Projeto>(p => p.Nome == novoProjetoCriado.Nome), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);

        // Act
        var result = await _incluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(novoProjetoCriado.Nome, result.Value.Nome);

        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Instancia_Projeto_For_Invalida()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new IncluirProjetoCommand(id, "", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));

        // Act
        var result = await _incluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Adicionar_Projeto_Via_Repositorio()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new IncluirProjetoCommand(id, "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));

        _projetoRepositoryMock.AdicionarProjetoAsync(Arg.Any<Projeto>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Projeto>>(_ => throw new Exception("Erro ao adicionar projeto"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirProjetoCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new IncluirProjetoCommand(id, "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30));

        var novoProjetoCriado = Projeto.Criar(id.ToProjetoId(), command.UsuarioId.ToUsuarioId(), command.Nome, command.Descricao, command.DataInicio, command.DataFim).Value;

        _projetoRepositoryMock.AdicionarProjetoAsync(Arg.Is<Projeto>(p => p.Id == novoProjetoCriado.Id), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao salvar no banco"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirProjetoCommandHandler.HandleAsync(command));
    }
}
