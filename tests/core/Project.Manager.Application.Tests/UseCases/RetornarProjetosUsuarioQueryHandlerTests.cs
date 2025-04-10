using Bogus;
using NSubstitute;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Identities;
using System.Text.RegularExpressions;

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

public static class UsuarioFaker
{
    public static Faker<Usuario> GerarUsuarioFake(Guid? usuarioId)
    {
        return new Faker<Usuario>("pt_BR")
            .CustomInstantiator(f =>
            {
                var senha = f.Internet.Password(length: 8);

                // Garante que tenha ao menos 1 letra maiúscula (por segurança extra)
                if (!Regex.IsMatch(senha, "[A-Z]"))
                    senha += "A";

                // Garante que tenha ao menos 1 número (por segurança extra)
                if (!Regex.IsMatch(senha, "[0-9]"))
                    senha += "1";

                var usuarioResult = Usuario.Criar(
                    new UsuarioId(usuarioId ?? f.Random.Guid()),
                    f.Name.FullName(),
                    f.Internet.Email(),
                    senha,
                    senha
                );

                if (usuarioResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar usuario fake.");

                return usuarioResult.Value;
            });
    }
}

public static class ProjetosFaker
{
    public static Faker<Projeto> GerarProjetosFakes(Guid usuarioId)
    {
        return new Faker<Projeto>("pt_BR")
            .CustomInstantiator(f =>
            {
                var usuario = UsuarioFaker.GerarUsuarioFake(usuarioId).Generate();

                var projetoResult = Projeto.Criar(
                    new ProjetoId(f.Random.Guid()),
                    usuario.Id, // usa o UsuarioId da entidade
                    f.Lorem.Sentence(),
                    f.Lorem.Paragraph(),
                    f.Date.Past(1),
                    f.Date.Future(1)
                );

                if (projetoResult.IsFailure)
                    throw new InvalidOperationException("Erro ao criar projeto fake.");

                var projeto = projetoResult.Value;
                projeto.SetUsuario(usuario);

                return projeto;
            });
    }
}
