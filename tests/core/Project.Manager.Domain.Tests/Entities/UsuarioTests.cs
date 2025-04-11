using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Tests.Entities;

public class UsuarioTests
{
    [Fact]
    public void Deve_Criar_Instancia_Usuario_Com_Sucesso()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "Senha123";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.NotNull(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsSuccess);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Nome_Esta_Vazio()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "Senha123";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(UsuarioErrors.NomeUsuarioNaoPodeSerVazio, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Email_Esta_Vazio()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "";
        var senha = "Senha123";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(EmailErrors.EmailNaoPodeSerVazio, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Email_E_Invalido()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste.com";
        var senha = "Senha123";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(EmailErrors.EmailInvalido, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Senha_Esta_Vazia()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(SenhaErrors.SenhaNaoPodeSerVazia, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Senha_E_Menor_Que_Tamanho_Minimo()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "senha";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(SenhaErrors.SenhaDeveTerTamanhoMinimo, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Senha_Deve_Ter_Pelo_Menos_Um_Caracter_Maiusculo()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "senha123";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(SenhaErrors.SenhaDeveTerUmaLetraMaiuscula, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Senha_Deve_Ter_Pelo_Menos_Um_Numero()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "SenhaUmDoisTres";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, senha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(SenhaErrors.SenhaDeveTerUmNumero, novaInstanciaUsuarioResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Senha_E_ConfirmarSenha_Forem_Diferentes()
    {
        // Arrange
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var email = "teste@gmail.com";
        var senha = "Senha123";
        var confirmarSenha = "Senha321";

        // Act
        var novaInstanciaUsuarioResult = Usuario.Criar(usuarioId, "Novo usuario", email, senha, confirmarSenha);

        // Assert
        Assert.Null(novaInstanciaUsuarioResult.Value);
        Assert.True(novaInstanciaUsuarioResult.IsFailure);
        Assert.Equal(SenhaErrors.SenhaNaoConfere, novaInstanciaUsuarioResult.Error);
    }

}
