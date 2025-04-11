using NSubstitute;
using Project.Manager.Application.Tests.Fakers;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.Tests.UseCases;
public class RetornarProjetosUsuarioQueryHandlerTests
{
    private readonly RetornarProjetosUsuarioQueryHandler _retornarProjetosUsuarioQueryHandler;
    private readonly IProjetoRepository _projetoRepositoryMock;

    public RetornarProjetosUsuarioQueryHandlerTests()
    {
        _projetoRepositoryMock = Substitute.For<IProjetoRepository>();

        _retornarProjetosUsuarioQueryHandler = new RetornarProjetosUsuarioQueryHandler(_projetoRepositoryMock);
    }

    [Fact]
    public async void Deve_Retornar_Projetos_Usuario_Com_Sucesso()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var query = new RetornarProjetosUsuarioQuery(usuarioId);
        var projetos = ProjetosFaker.GerarProjetosFakes(usuarioId).Generate(5);

        _projetoRepositoryMock.RetornarProjetosUsuarioAsync(Arg.Is<UsuarioId>(id => id == new UsuarioId(usuarioId)), Arg.Any<CancellationToken>())
            .Returns(projetos);

        // Act
        var result = await _retornarProjetosUsuarioQueryHandler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(projetos.Count, result.Value.Count());
    }

    [Fact]
    public async void Deve_Retornar_Erro_Quando_Nao_Houver_Projetos_Cadastrados_Para_Usuario()
    {
        // Arrange
        var usuarioId = Guid.NewGuid();
        var query = new RetornarProjetosUsuarioQuery(usuarioId);
        _projetoRepositoryMock.RetornarProjetosUsuarioAsync(Arg.Is<UsuarioId>(id => id == new UsuarioId(usuarioId)), Arg.Any<CancellationToken>())
            .Returns([]);

        // Act
        var result = await _retornarProjetosUsuarioQueryHandler.HandleAsync(query);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsFailure);
        Assert.Equal(ProjetoErrors.NaoExistemProjetosCadastradosParaUsuarioInformado, result.Error);
    }
}