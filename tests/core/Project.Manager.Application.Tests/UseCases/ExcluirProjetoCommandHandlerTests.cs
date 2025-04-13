using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Application.Tests.Fakers;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class ExcluirProjetoCommandHandlerTests
{
    private readonly ExcluirProjetoCommandHandler _excluirProjetoCommandHandler;
    private readonly IProjetoRepository _projetoRepositoryMock;
    private readonly ITarefaRepository _tarefaRepositoryMock;
    private readonly IUnityOfWork _unityOfWorkMock;

    public ExcluirProjetoCommandHandlerTests()
    {
        _projetoRepositoryMock = Substitute.For<IProjetoRepository>();
        _tarefaRepositoryMock = Substitute.For<ITarefaRepository>();
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();

        _excluirProjetoCommandHandler = new ExcluirProjetoCommandHandler(_projetoRepositoryMock, _tarefaRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Excluir_Projeto_Com_Sucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new ExcluirProjetoCommand(id);
        var projeto = Projeto.Criar(id.ToProjetoId(), usuarioId.ToUsuarioId(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30)).Value;
        var tarefas = TarefasFaker.GerarTarefasStatusFakes(id, StatusTarefa.Concluida).Generate(5);

        tarefas.ForEach(tarefa =>
        {
            projeto.AddTarefa(tarefa);
        });

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(projeto);

        _tarefaRepositoryMock.RetornaTarefasPorProjetoAsync(projeto.Id, Arg.Any<CancellationToken>())
            .Returns(tarefas);

        // Act
        var result = await _excluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);

        await _projetoRepositoryMock.Received(1).RemoverProjetoAsync(projeto, Arg.Any<CancellationToken>());
        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Projeto_Nao_Encontrado()
    {
        // Arrange
        var id = Guid.NewGuid();
        var command = new ExcluirProjetoCommand(id);

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == id.ToProjetoId()), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _excluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProjetoErrors.ProjetoNaoEncontrado, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Tiver_Tarefas_Pendentes()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new ExcluirProjetoCommand(id);
        var projeto = Projeto.Criar(id.ToProjetoId(), usuarioId.ToUsuarioId(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30)).Value;
        var tarefas = TarefasFaker.GerarTarefasFakes(id).Generate(5);

        tarefas.ForEach(tarefa =>
        {
            projeto.AddTarefa(tarefa);
        });

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(projeto);

        _tarefaRepositoryMock.RetornaTarefasPorProjetoAsync(projeto.Id, Arg.Any<CancellationToken>())
            .Returns(tarefas);

        // Act
        var result = await _excluirProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Equal(ProjetoErrors.TarefasPendentesConclusao, result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Excluir_Projeto_Via_Repositorio()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new ExcluirProjetoCommand(id);
        var projeto = Projeto.Criar(id.ToProjetoId(), usuarioId.ToUsuarioId(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30)).Value;
        var tarefas = TarefasFaker.GerarTarefasStatusFakes(id, StatusTarefa.Concluida).Generate(5);

        tarefas.ForEach(tarefa =>
        {
            projeto.AddTarefa(tarefa);
        });

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(projeto);

        _tarefaRepositoryMock.RetornaTarefasPorProjetoAsync(projeto.Id, Arg.Any<CancellationToken>())
            .Returns(tarefas);

        _projetoRepositoryMock.RemoverProjetoAsync(Arg.Is<Projeto>(p => p == projeto), Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao adicionar projeto"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _excluirProjetoCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var command = new ExcluirProjetoCommand(id);
        var projeto = Projeto.Criar(id.ToProjetoId(), usuarioId.ToUsuarioId(), "Projeto Teste", "Descrição do projeto", DateTime.Now, DateTime.Now.AddDays(30)).Value;
        var tarefas = TarefasFaker.GerarTarefasStatusFakes(id, StatusTarefa.Concluida).Generate(5);

        tarefas.ForEach(tarefa =>
        {
            projeto.AddTarefa(tarefa);
        });

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(projeto);

        _tarefaRepositoryMock.RetornaTarefasPorProjetoAsync(projeto.Id, Arg.Any<CancellationToken>())
            .Returns(tarefas);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao adicionar projeto"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _excluirProjetoCommandHandler.HandleAsync(command));
    }
}