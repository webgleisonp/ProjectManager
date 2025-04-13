using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Identities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Manager.Domain.Tests.Entities;

public class HistoricoTests
{
    [Fact]
    public void Deve_Criar_Instancia_Atualizacao_Com_Sucesso()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarAtualizacao(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.NotNull(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsSuccess);
        Assert.Equal(historicoId, novaInstanciaHistoricoResult.Value.Id);
    }

    [Fact]
    public void Deve_Criar_Instancia_Comentario_Com_Sucesso()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarComentario(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.NotNull(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsSuccess);
        Assert.Equal(historicoId, novaInstanciaHistoricoResult.Value.Id);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Atualicacao_Id_Esta_Vazio()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.Empty);
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarAtualizacao(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(HistoricoErrors.IdHistoricoNaoPodeSerVazio, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Atualicacao_IdTarefa_Esta_Vazio()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.Empty);
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarAtualizacao(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(TarefaErrors.IdTarefaNaoPodeSerVazio, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Atualicacao_IdUsuario_Esta_Vazio()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.Empty);
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarAtualizacao(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(UsuarioErrors.IdUsuarioNaoPodeSerVazio, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Atualicacao_Descricao_Esta_Vazia()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarAtualizacao(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(HistoricoErrors.DescricaoHistoricoNaoPodeSerVazia, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Comentario_Id_Esta_Vazio()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.Empty);
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarComentario(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(HistoricoErrors.IdHistoricoNaoPodeSerVazio, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Comentario_IdTarefa_Esta_Vazio()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.Empty);
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarComentario(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(TarefaErrors.IdTarefaNaoPodeSerVazio, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Comentario_IdUsuario_Esta_Vazio()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.Empty);
        var descricao = "Descrição do histórico";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarComentario(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(UsuarioErrors.IdUsuarioNaoPodeSerVazio, novaInstanciaHistoricoResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Criar_Comentario_Descricao_Esta_Vazia()
    {
        // Arrange
        var historicoId = new HistoricoId(Guid.NewGuid());
        var tarefaId = new TarefaId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var descricao = "";

        // Act
        var novaInstanciaHistoricoResult = Historico.CriarComentario(historicoId, tarefaId, usuarioId, descricao);

        // Assert
        Assert.Null(novaInstanciaHistoricoResult.Value);
        Assert.True(novaInstanciaHistoricoResult.IsFailure);
        Assert.Equal(HistoricoErrors.DescricaoHistoricoNaoPodeSerVazia, novaInstanciaHistoricoResult.Error);
    }
}
