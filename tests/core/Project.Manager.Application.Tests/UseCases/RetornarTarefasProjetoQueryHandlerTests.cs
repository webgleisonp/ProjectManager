using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Project.Manager.Application.Tests.Fakers;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;

public class RetornarTarefasProjetoQueryHandlerTests
{
    private readonly RetornarTarefasProjetoQueryHandler _retornarTarefasProjetoQueryHandler;
    private readonly IProjetoRepository _projetoRepositoryMock;
    private readonly ITarefaRepository _tarefaRepositoryMock;

    public RetornarTarefasProjetoQueryHandlerTests()
    {
        _projetoRepositoryMock = Substitute.For<IProjetoRepository>();
        _tarefaRepositoryMock = Substitute.For<ITarefaRepository>();

        _retornarTarefasProjetoQueryHandler = new RetornarTarefasProjetoQueryHandler(_projetoRepositoryMock, _tarefaRepositoryMock);
    }

    [Fact]
    public async void Deve_Retornar_Tarefas_Projeto_Com_Sucesso()
    {
        // Arrange
        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();
        var query = new RetornarTarefasProjetoQuery(projeto.Id.Value);
        var tarefas = TarefasFaker.GerarTarefasFakes(projeto.Id.Value).Generate(20);

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(id => id == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(projeto);

        _tarefaRepositoryMock.RetornaTarefasPorProjetoAsync(Arg.Is<ProjetoId>(id => id == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(tarefas);

        // Act
        var result = await _retornarTarefasProjetoQueryHandler.HandleAsync(query);

        // Assert
        Assert.NotNull(result.Value);
        Assert.True(result.IsSuccess);
        Assert.Equal(tarefas.Count, result.Value.Tarefas.Count());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Projeto_Nao_Encontrado()
    {
        // Arrange
        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();
        var query = new RetornarTarefasProjetoQuery(projeto.Id.Value);

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(id => id == projeto.Id), Arg.Any<CancellationToken>())
            .ReturnsNull();

        // Act
        var result = await _retornarTarefasProjetoQueryHandler.HandleAsync(query);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.Equal(ProjetoErrors.ProjetoNaoEncontrado, result.Error);
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Tarefas_Nao_Encontradas()
    {
        // Arrange
        var projeto = ProjetosFaker.GerarProjetosFakes().Generate();
        var query = new RetornarTarefasProjetoQuery(projeto.Id.Value);
        var tarefas = TarefasFaker.GerarTarefasFakes(projeto.Id.Value).Generate(20);

        _projetoRepositoryMock.RetornarProjetoAsync(Arg.Is<ProjetoId>(id => id == projeto.Id), Arg.Any<CancellationToken>())
            .Returns(projeto);

        _tarefaRepositoryMock.RetornaTarefasPorProjetoAsync(Arg.Is<ProjetoId>(id => id == projeto.Id), Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        var result = await _retornarTarefasProjetoQueryHandler.HandleAsync(query);

        // Assert
        Assert.Null(result.Value);
        Assert.True(result.IsFailure);
        Assert.Equal(TarefaErrors.NenhumaTarefaEncontradaParaProjetoInformado, result.Error);
    }
}
