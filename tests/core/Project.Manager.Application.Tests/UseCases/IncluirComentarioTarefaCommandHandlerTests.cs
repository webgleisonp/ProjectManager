using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Application.Tests.Fakers;
using Project.Manager.Application.UseCases.Comentarios;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;

namespace Project.Manager.Application.Tests.UseCases;

public class IncluirComentarioTarefaCommandHandlerTests
{
    private readonly IncluirComentarioTarefaCommandHandler incluirComentarioTarefaCommandHandler;
    private readonly ITarefaRepository _tarefaRepositoryMock;
    private readonly IUsuarioRepository _usuarioRepositoryMock;
    private readonly IHistoricoRepository _historicoRepositoryMock;
    private readonly IUnityOfWork _unityOfWorkMock;

    public IncluirComentarioTarefaCommandHandlerTests()
    {
        _tarefaRepositoryMock = Substitute.For<ITarefaRepository>();
        _usuarioRepositoryMock = Substitute.For<IUsuarioRepository>();
        _historicoRepositoryMock = Substitute.For<IHistoricoRepository>();
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();

        incluirComentarioTarefaCommandHandler = new IncluirComentarioTarefaCommandHandler(_tarefaRepositoryMock, _usuarioRepositoryMock, _historicoRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Incluir_Comentario_Tarefa_Com_Sucesso()
    {
        // Arrange
        var tarefa = TarefasFaker.GerarTarefasFakes().Generate();
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();
        var historico = HistoricoFaker.GerarHistoricoFake().Generate();

        var command = new IncluirComentarioTarefaCommand(tarefa.Id.Value, usuario.Id.Value, historico.Descricao);

        _tarefaRepositoryMock.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        _usuarioRepositoryMock.RetornarUsuarioAsync(command.UsuarioId.ToUsuarioId(), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _historicoRepositoryMock.AdicionarHistoricoAsync(Arg.Any<Historico>(), Arg.Any<CancellationToken>())
            .Returns(historico);

        // Act
        var result = await incluirComentarioTarefaCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(command.Comentario, result.Value.Comentario);

        await _historicoRepositoryMock.Received(1).AdicionarHistoricoAsync(Arg.Any<Historico>(), Arg.Any<CancellationToken>());
        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Tarefa_Nao_Encontrada()
    {
        // Arrange
        var command = new IncluirComentarioTarefaCommand(Guid.NewGuid(), Guid.NewGuid(), "Comentario");

        _tarefaRepositoryMock.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await incluirComentarioTarefaCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(TarefaErrors.TarefaNaoEncontrada, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Usuario_Nao_Encontrado()
    {
        // Arrange
        var tarefa = TarefasFaker.GerarTarefasFakes().Generate();

        var command = new IncluirComentarioTarefaCommand(tarefa.Id.Value, Guid.NewGuid(), "Comentario");

        _tarefaRepositoryMock.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        // Act
        var result = await incluirComentarioTarefaCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(UsuarioErrors.UsuarioNaoEncontrado, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Comentario_Invalido()
    {
        // Arrange
        var tarefa = TarefasFaker.GerarTarefasFakes().Generate();
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();
        var command = new IncluirComentarioTarefaCommand(tarefa.Id.Value, Guid.NewGuid(), string.Empty);

        _tarefaRepositoryMock.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        _usuarioRepositoryMock.RetornarUsuarioAsync(command.UsuarioId.ToUsuarioId(), Arg.Any<CancellationToken>())
            .Returns(usuario);

        // Act
        var result = await incluirComentarioTarefaCommandHandler.HandleAsync(command);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(HistoricoErrors.DescricaoHistoricoNaoPodeSerVazia, result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Incluir_Comentario_Via_Repositorio()
    {
        // Arrange
        var tarefa = TarefasFaker.GerarTarefasFakes().Generate();
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();
        var command = new IncluirComentarioTarefaCommand(tarefa.Id.Value, Guid.NewGuid(), "Comentario");

        _tarefaRepositoryMock.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        _usuarioRepositoryMock.RetornarUsuarioAsync(command.UsuarioId.ToUsuarioId(), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _historicoRepositoryMock.AdicionarHistoricoAsync(Arg.Any<Historico>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Historico>>(_ => throw new Exception("Erro ao incluir comentário"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await incluirComentarioTarefaCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var tarefa = TarefasFaker.GerarTarefasFakes().Generate();
        var usuario = UsuarioFaker.GerarUsuarioFake().Generate();
        var historico = HistoricoFaker.GerarHistoricoFake().Generate();
        var command = new IncluirComentarioTarefaCommand(tarefa.Id.Value, Guid.NewGuid(), "Comentario");

        _tarefaRepositoryMock.RetornarTarefaAsync(command.TarefaId.ToTarefaId(), Arg.Any<CancellationToken>())
            .Returns(tarefa);

        _usuarioRepositoryMock.RetornarUsuarioAsync(command.UsuarioId.ToUsuarioId(), Arg.Any<CancellationToken>())
            .Returns(usuario);

        _historicoRepositoryMock.AdicionarHistoricoAsync(Arg.Any<Historico>(), Arg.Any<CancellationToken>())
            .Returns(historico);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao salvar alterações"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await incluirComentarioTarefaCommandHandler.HandleAsync(command));
    }
}
