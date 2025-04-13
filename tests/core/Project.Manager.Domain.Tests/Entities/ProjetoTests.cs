using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Tests.Entities;

public class ProjetoTests
{
    [Fact]
    public void Deve_Criar_Instancia_De_Projeto_Com_Sucesso()
    {
        // Arrange
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Novo Projeto";
        var descricao = "Descrição do projeto";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);

        // Act
        var novaInstanciaProjetoResult = Projeto.Criar(projetoId, usuarioId, nome, descricao, dataInicio, dataFim);

        // Assert
        Assert.NotNull(novaInstanciaProjetoResult.Value);
        Assert.True(novaInstanciaProjetoResult.IsSuccess);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Esta_Vazio()
    {
        // Arrange
        var projetoId = new ProjetoId(Guid.Empty);
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Novo Projeto";
        var descricao = "Descrição do projeto";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);

        // Act
        var novaInstanciaProjetoResult = Projeto.Criar(projetoId, usuarioId, nome, descricao, dataInicio, dataFim);

        // Assert
        Assert.Null(novaInstanciaProjetoResult.Value);
        Assert.True(novaInstanciaProjetoResult.IsFailure);
        Assert.Equal(ProjetoErrors.IdProjetoNaoPodeSerVazio, novaInstanciaProjetoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_IdUsuario_Esta_Vazio()
    {
        // Arrange
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.Empty);
        var nome = "Novo Projeto";
        var descricao = "Descrição do projeto";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);

        // Act
        var novaInstanciaProjetoResult = Projeto.Criar(projetoId, usuarioId, nome, descricao, dataInicio, dataFim);

        // Assert
        Assert.Null(novaInstanciaProjetoResult.Value);
        Assert.True(novaInstanciaProjetoResult.IsFailure);
        Assert.Equal(UsuarioErrors.IdUsuarioNaoPodeSerVazio, novaInstanciaProjetoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Nome_Esta_Vazio()
    {
        // Arrange
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "";
        var descricao = "Descrição do projeto";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);

        // Act
        var novaInstanciaProjetoResult = Projeto.Criar(projetoId, usuarioId, nome, descricao, dataInicio, dataFim);

        // Assert
        Assert.Null(novaInstanciaProjetoResult.Value);
        Assert.True(novaInstanciaProjetoResult.IsFailure);
        Assert.Equal(ProjetoErrors.NomeProjetoNaoPodeSerVazio, novaInstanciaProjetoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Descricao_Esta_Vazio()
    {
        // Arrange
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Novo Projeto";
        var descricao = "";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);

        // Act
        var novaInstanciaProjetoResult = Projeto.Criar(projetoId, usuarioId, nome, descricao, dataInicio, dataFim);

        // Assert
        Assert.Null(novaInstanciaProjetoResult.Value);
        Assert.True(novaInstanciaProjetoResult.IsFailure);
        Assert.Equal(ProjetoErrors.DescricaoProjetoNaoPodeSerVazia, novaInstanciaProjetoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_DataInicio_For_Maior_Ou_Igual_DataFim()
    {
        // Arrange
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Novo Projeto";
        var descricao = "Descrição do projeto";
        var dataInicio = DateTime.UtcNow;
        var dataFim = dataInicio;

        // Act
        var novaInstanciaProjetoResult = Projeto.Criar(projetoId, usuarioId, nome, descricao, dataInicio, dataFim);

        // Assert
        Assert.Null(novaInstanciaProjetoResult.Value);
        Assert.True(novaInstanciaProjetoResult.IsFailure);
        Assert.Equal(ProjetoErrors.DataInicioDeveSerMenorQueDataFim, novaInstanciaProjetoResult.Error);
    }
}
