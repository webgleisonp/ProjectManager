using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class RemoverTarefaProjetoCommandHandlerTests
{
    private readonly RemoverTarefaProjetoCommandHandler _removerTarefaProjetoCommandHandler;
    private readonly ITarefaRepository _tarefaRepositoryMock;
    private readonly IUnityOfWork _unityOfWorkMock;

    public RemoverTarefaProjetoCommandHandlerTests()
    {
        _tarefaRepositoryMock = Substitute.For<ITarefaRepository>();
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();

        _removerTarefaProjetoCommandHandler = new RemoverTarefaProjetoCommandHandler(_tarefaRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Remover_Tarefa_Com_Sucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new RemoverTarefaProjetoCommand(id);

        var tarefa = Tarefa.Criar(id.ToTarefaId(),
            projetoId.ToProjetoId(),
            usuarioId.ToUsuarioId(),
            "Tarefa Teste",
            "Descrição da Tarefa Teste",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(5),
            StatusTarefa.Pendente,
            PrioridadeTarefa.Baixa).Value;

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == id.ToTarefaId()), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        // Act
        var result = await _removerTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);

        await _tarefaRepositoryMock.Received(1).RemoverTarefaProjetoAsync(Arg.Is<Tarefa>(t => t.Id == id.ToTarefaId()), Arg.Any<CancellationToken>());
        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Tarefa_Nao_Encontrada()
    {
        // Arrange
        var id = Guid.NewGuid();

        var command = new RemoverTarefaProjetoCommand(id);

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == id.ToTarefaId()), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _removerTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(TarefaErrors.TarefaNaoEncontrada, result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Remover_Tarefa_Via_Repositorio()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new RemoverTarefaProjetoCommand(id);

        var tarefa = Tarefa.Criar(id.ToTarefaId(),
            projetoId.ToProjetoId(),
            usuarioId.ToUsuarioId(),
            "Tarefa Teste",
            "Descrição da Tarefa Teste",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(5),
            StatusTarefa.Pendente,
            PrioridadeTarefa.Baixa).Value;

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == id.ToTarefaId()), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        _tarefaRepositoryMock.RemoverTarefaProjetoAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Falha no repositório"));

        _tarefaRepositoryMock.When(x => x.RemoverTarefaProjetoAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>()))
            .Do(x => throw new Exception("Erro ao remover tarefa"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _removerTarefaProjetoCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        var projetoId = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new RemoverTarefaProjetoCommand(Guid.NewGuid());

        var tarefa = Tarefa.Criar(id.ToTarefaId(),
            projetoId.ToProjetoId(),
            usuarioId.ToUsuarioId(),
            "Tarefa Teste",
            "Descrição da Tarefa Teste",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(5),
            StatusTarefa.Pendente,
            PrioridadeTarefa.Baixa).Value;

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == id.ToTarefaId()), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Falha ao salvar mudanças"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _removerTarefaProjetoCommandHandler.HandleAsync(command));
    }
}
