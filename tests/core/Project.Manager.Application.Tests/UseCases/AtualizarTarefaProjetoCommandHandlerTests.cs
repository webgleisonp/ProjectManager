using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Application.Tests.Fakers;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class AtualizarTarefaProjetoCommandHandlerTests
{
    private readonly AtualizarTarefaProjetoCommandHandler _atualizarTarefaProjetoCommandHandler;
    private readonly ITarefaRepository _tarefaRepositoryMock;
    private readonly IUsuarioRepository _usuarioRepositoryMock;
    private readonly IUnityOfWork _unityOfWorkMock;

    public AtualizarTarefaProjetoCommandHandlerTests()
    {
        _tarefaRepositoryMock = Substitute.For<ITarefaRepository>();
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();

        _atualizarTarefaProjetoCommandHandler = new AtualizarTarefaProjetoCommandHandler(_tarefaRepositoryMock, _usuarioRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Atualizar_Tarefa_Com_Sucesso()
    {
        // Arrange
        var id = Guid.NewGuid();
        
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();

        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();

        var command = new AtualizarTarefaProjetoCommand(id, usuario.Id.Value, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), StatusTarefa.EmAndamento);

        var tarefaJaIncluida = Tarefa.Criar(id.ToTarefaId(),
            projeto.Id,
            usuario.Id,
            "Tarefa Incluida",
            "Descrição Incluida",
            DateTime.UtcNow,
            DateTime.UtcNow.AddDays(5),
            StatusTarefa.Pendente,
            PrioridadeTarefa.Media).Value;

        tarefaJaIncluida.SetProjeto(projeto);

        var tarefaAtualizada = Tarefa.Criar(id.ToTarefaId(),
            projeto.Id,
            usuario.Id,
            command.Nome,
            command.Descricao,
            command.DataInicio,
            command.DataFim,
            command.Status,
            PrioridadeTarefa.Media).Value;

        tarefaAtualizada.SetProjeto(projeto);

        _usuarioRepositoryMock.RetornarUsuarioAsync(Arg.Is<UsuarioId>(t => t == usuario.Id), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == tarefaJaIncluida.Id), Arg.Any<CancellationToken>())
            .Returns(tarefaJaIncluida);

        _tarefaRepositoryMock.AtualizarTarefaAsync(Arg.Is<Tarefa>(t => t.Id == tarefaAtualizada.Id), Arg.Any<CancellationToken>())
            .Returns(tarefaAtualizada);

        // Act
        var result = await _atualizarTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Nome, tarefaAtualizada.Nome);
        Assert.Equal(command.Descricao, tarefaAtualizada.Descricao);
        Assert.Equal(command.DataInicio, tarefaAtualizada.DataInicio);
        Assert.Equal(command.DataFim, tarefaAtualizada.DataFim);
        Assert.Equal(command.Status, tarefaAtualizada.Status);
        Assert.Equal(tarefaJaIncluida.Prioridade, tarefaAtualizada.Prioridade);

        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Usuario_Nao_Encontrado()
    {
        // Arrange
        var id = Guid.NewGuid();

        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();

        var command = new AtualizarTarefaProjetoCommand(id, usuario.Id.Value, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), StatusTarefa.EmAndamento);
        
        _usuarioRepositoryMock.RetornarUsuarioAsync(Arg.Is<UsuarioId>(t => t == usuario.Id), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _atualizarTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UsuarioErrors.UsuarioNaoEncontrado, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Tarefa_Nao_Encontrada()
    {
        // Arrange
        var id = Guid.NewGuid();

        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();

        var command = new AtualizarTarefaProjetoCommand(id, usuario.Id.Value, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), StatusTarefa.EmAndamento);

        _usuarioRepositoryMock.RetornarUsuarioAsync(Arg.Is<UsuarioId>(t => t == usuario.Id), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == id.ToTarefaId()), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _atualizarTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(TarefaErrors.TarefaNaoEncontrada, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_DataInicio_For_Maior_Ou_Igual_DataFim()
    {
        // Arrange
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();

        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();

        var tarefaJaIncluida = TarefasFaker.GerarTarefasFakes(projeto.Id.Value).Generate();

        var command = new AtualizarTarefaProjetoCommand(tarefaJaIncluida.Id.Value, usuario.Id.Value, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow.AddDays(5), DateTime.UtcNow, StatusTarefa.EmAndamento);

        _usuarioRepositoryMock.RetornarUsuarioAsync(Arg.Is<UsuarioId>(t => t == usuario.Id), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == tarefaJaIncluida.Id), Arg.Any<CancellationToken>())
            .Returns(tarefaJaIncluida);

        // Act
        var result = await _atualizarTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(TarefaErrors.DataInicioDeveSerMenorQueDataFim, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_DataFim_For_Menor_Ou_Igual_DataInicio()
    {
        // Arrange
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();

        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();

        var tarefaJaIncluida = TarefasFaker.GerarTarefasFakes(projeto.Id.Value).Generate();

        var command = new AtualizarTarefaProjetoCommand(tarefaJaIncluida.Id.Value, usuario.Id.Value, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow, DateTime.UtcNow.AddDays(-1), StatusTarefa.EmAndamento);

        _usuarioRepositoryMock.RetornarUsuarioAsync(Arg.Is<UsuarioId>(t => t == usuario.Id), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == tarefaJaIncluida.Id), Arg.Any<CancellationToken>())
            .Returns(tarefaJaIncluida);

        // Act
        var result = await _atualizarTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(TarefaErrors.DataInicioDeveSerMenorQueDataFim, result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Atualizar_Tarefa_Via_Repositorio()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();

        var command = new AtualizarTarefaProjetoCommand(id, usuarioId, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), StatusTarefa.EmAndamento);

        var tarefaJaIncluida = TarefasFaker.GerarTarefasFakes(projeto.Id.Value).Generate();

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == tarefaJaIncluida.Id), Arg.Any<CancellationToken>())
            .Returns(tarefaJaIncluida);

        _tarefaRepositoryMock.AtualizarTarefaAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Tarefa>>(_ => throw new Exception("Falha no repositório"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _atualizarTarefaProjetoCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        var usuarioId = Guid.NewGuid();

        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();

        var command = new AtualizarTarefaProjetoCommand(id, usuarioId, "Tarefa Atualizada", "Descrição Atualizada", DateTime.UtcNow, DateTime.UtcNow.AddDays(5), StatusTarefa.EmAndamento);

        var tarefaJaIncluida = TarefasFaker.GerarTarefasFakes(projeto.Id.Value).Generate();

        _tarefaRepositoryMock.RetornarTarefaAsync(Arg.Is<TarefaId>(t => t == tarefaJaIncluida.Id), Arg.Any<CancellationToken>())
            .Returns(tarefaJaIncluida);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Falha ao salvar mudanças"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _atualizarTarefaProjetoCommandHandler.HandleAsync(command));
    }
}