using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Tests.Fakers;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class IncluirTarefaProjetoCommandHandlerTests
{
    private readonly IncluirTarefaProjetoCommandHandler _incluirTarefaProjetoCommandHandler;
    private readonly IProjetoRepository _projetoRepositoryMock;
    private readonly ITarefaRepository _tarefaRepositoryMock;
    private readonly IUnityOfWork _unityOfWorkMock;

    public IncluirTarefaProjetoCommandHandlerTests()
    {
        _projetoRepositoryMock = Substitute.For<IProjetoRepository>();
        _tarefaRepositoryMock = Substitute.For<ITarefaRepository>();
        _unityOfWorkMock = Substitute.For<IUnityOfWork>();

        _incluirTarefaProjetoCommandHandler = new IncluirTarefaProjetoCommandHandler(_projetoRepositoryMock, _tarefaRepositoryMock, _unityOfWorkMock);
    }

    [Fact]
    public async void Deve_Incluir_Tarefa_Com_Sucesso()
    {
        // Arrange
        var command = new IncluirTarefaProjetoCommand(Guid.NewGuid(), 
            "Tarefa Teste", 
            "Descrição da Tarefa Teste", 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(5),
            PrioridadeTarefa.Baixa);

        var novoProjetoCriado = Projeto.Criar(new ProjetoId(command.ProjetoId), 
            new UsuarioId(Guid.NewGuid()), 
            "Projeto Teste", 
            "Descrição do projeto", 
            DateTime.Now, 
            DateTime.Now.AddDays(30)).Value;

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == novoProjetoCriado.Id), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);

        var novaTarefaCriada = Tarefa.Criar(new TarefaId(Guid.NewGuid()),
            novoProjetoCriado.Id, 
            command.Nome,
            command.Descricao, 
            command.DataInicio, 
            command.DataFim, 
            StatusTarefa.Pendente,
            command.Prioridade).Value;

        _tarefaRepositoryMock.AdicionarTarefaAsync(Arg.Is<Tarefa>(t => t.Nome == novaTarefaCriada.Nome), Arg.Any<CancellationToken>())
                    .Returns(novaTarefaCriada);

        // Act
        var result = await _incluirTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(novaTarefaCriada.Nome, result.Value.Nome);

        await _unityOfWorkMock.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Instancia_Tarefa_For_Invalida()
    {
        // Arrange
        var command = new IncluirTarefaProjetoCommand(Guid.NewGuid(), 
            "", 
            "Descrição da Tarefa Teste", 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(5), 
            PrioridadeTarefa.Baixa);

        var novoProjetoCriado = Projeto.Criar(new ProjetoId(command.ProjetoId), 
            new UsuarioId(Guid.NewGuid()), 
            "Projeto Teste", 
            "Descrição do projeto", 
            DateTime.Now, 
            DateTime.Now.AddDays(30)).Value;

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == novoProjetoCriado.Id), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);

        // Act
        var result = await _incluirTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Projeto_Nao_Encontrado()
    {
        // Arrange
        var command = new IncluirTarefaProjetoCommand(Guid.NewGuid(), 
            "Tarefa Teste", 
            "Descrição da Tarefa Teste", 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(5),
            PrioridadeTarefa.Baixa);

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == new ProjetoId(command.ProjetoId)), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _incluirTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(ProjetoErrors.ProjetoNaoEncontrado, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Limite_Tarefas_Por_Projeto_For_Atingido()
    {
        // Arrange
        var command = new IncluirTarefaProjetoCommand(Guid.NewGuid(), 
            "Tarefa Teste", 
            "Descrição da Tarefa Teste", 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(5), 
            PrioridadeTarefa.Baixa);

        var novoProjetoCriado = Projeto.Criar(new ProjetoId(command.ProjetoId), 
            new UsuarioId(Guid.NewGuid()), 
            "Projeto Teste", 
            "Descrição do projeto", 
            DateTime.Now, 
            DateTime.Now.AddDays(30)).Value;

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == novoProjetoCriado.Id), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);
        
        var tarefas = TarefasFaker.GerarTarefasFakes(novoProjetoCriado.Id.Value).Generate(20);

        foreach (var tarefa in tarefas)
        {
            novoProjetoCriado.AddTarefa(tarefa);
        }
        
        // Adicione mais tarefas até atingir o limite de 20
        // Act
        var result = await _incluirTarefaProjetoCommandHandler.HandleAsync(command);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Equal(ProjetoErrors.LimiteDeTarefasPorProjeto, result.Error);
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Adicionar_Tarefa_Via_Repositorio()
    {
        // Arrange
        var command = new IncluirTarefaProjetoCommand(Guid.NewGuid(), 
            "Tarefa Teste", 
            "Descrição da Tarefa Teste", 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(5), 
            PrioridadeTarefa.Baixa);
        
        var novoProjetoCriado = Projeto.Criar(new ProjetoId(command.ProjetoId), 
            new UsuarioId(Guid.NewGuid()), 
            "Projeto Teste", 
            "Descrição do projeto", 
            DateTime.Now, 
            DateTime.Now.AddDays(30)).Value;

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == novoProjetoCriado.Id), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);

        _tarefaRepositoryMock.AdicionarTarefaAsync(Arg.Any<Tarefa>(), Arg.Any<CancellationToken>())
            .Returns<ValueTask<Tarefa>>(_ => throw new Exception("Falha no repositório"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirTarefaProjetoCommandHandler.HandleAsync(command));
    }

    [Fact]
    public void Deve_Lancar_Exception_Quando_Executar_SaveChanges()
    {
        // Arrange
        var command = new IncluirTarefaProjetoCommand(Guid.NewGuid(), 
            "Tarefa Teste", 
            "Descrição da Tarefa Teste", 
            DateTime.UtcNow, 
            DateTime.UtcNow.AddDays(5),
            PrioridadeTarefa.Baixa);
        
        var novoProjetoCriado = Projeto.Criar(new ProjetoId(command.ProjetoId),
            new UsuarioId(Guid.NewGuid()), 
            "Projeto Teste", 
            "Descrição do projeto", 
            DateTime.Now, 
            DateTime.Now.AddDays(30)).Value;
        
        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(p => p == novoProjetoCriado.Id), Arg.Any<CancellationToken>())
            .Returns(novoProjetoCriado);

        _unityOfWorkMock.SaveChangesAsync(Arg.Any<CancellationToken>())
            .Returns(_ => throw new Exception("Erro ao salvar no banco"));

        // Act & Assert
        Assert.ThrowsAsync<Exception>(async () => await _incluirTarefaProjetoCommandHandler.HandleAsync(command));
    }
}
