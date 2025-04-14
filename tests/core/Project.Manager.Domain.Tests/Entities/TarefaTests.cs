using Project.Manager.Domain.Entities;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.ValueObjects.Enums;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Domain.Tests.Entities;

public class TarefaTests
{
    [Fact]
    public void Deve_Criar_Instancia_Tarefa_Com_Sucesso()
    {
        // Arrange
        var tarefaId = new TarefaId(Guid.NewGuid());
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Nova Tarefa";
        var descricao = "Descrição da tarefa";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);
        var status = StatusTarefa.Pendente;
        var prioridade = PrioridadeTarefa.Baixa;

        // Act
        var novaInstanciaTarefaResult = Tarefa.Criar(tarefaId, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade);

        // Assert
        Assert.NotNull(novaInstanciaTarefaResult.Value);
        Assert.True(novaInstanciaTarefaResult.IsSuccess);
        Assert.Equal(tarefaId, novaInstanciaTarefaResult.Value.Id);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Id_Esta_Vazio()
    {
        // Arrange
        var tarefaId = new TarefaId(Guid.Empty);
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Nova Tarefa";
        var descricao = "Descrição da tarefa";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);
        var status = StatusTarefa.Pendente;
        var prioridade = PrioridadeTarefa.Baixa;

        // Act
        var novaInstanciaTarefaResult = Tarefa.Criar(tarefaId, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade);

        // Assert
        Assert.Null(novaInstanciaTarefaResult.Value);
        Assert.True(novaInstanciaTarefaResult.IsFailure);
        Assert.Equal(TarefaErrors.IdTarefaNaoPodeSerVazio, novaInstanciaTarefaResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_IdProjeto_Esta_Vazio()
    {
        // Arrange
        var tarefaId = new TarefaId(Guid.NewGuid());
        var projetoId = new ProjetoId(Guid.Empty);
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Nova Tarefa";
        var descricao = "Descrição da tarefa";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);
        var status = StatusTarefa.Pendente;
        var prioridade = PrioridadeTarefa.Baixa;

        // Act
        var novaInstanciaTarefaResult = Tarefa.Criar(tarefaId, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade);

        // Assert
        Assert.Null(novaInstanciaTarefaResult.Value);
        Assert.True(novaInstanciaTarefaResult.IsFailure);
        Assert.Equal(ProjetoErrors.IdProjetoNaoPodeSerVazio, novaInstanciaTarefaResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Nome_Esta_Vazio()
    {
        // Arrange
        var tarefaId = new TarefaId(Guid.NewGuid());
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "";
        var descricao = "Descrição da tarefa";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);
        var status = StatusTarefa.Pendente;
        var prioridade = PrioridadeTarefa.Baixa;

        // Act
        var novaInstanciaTarefaResult = Tarefa.Criar(tarefaId, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade);

        // Assert
        Assert.Null(novaInstanciaTarefaResult.Value);
        Assert.True(novaInstanciaTarefaResult.IsFailure);
        Assert.Equal(TarefaErrors.NomeTarefaNaoPodeSerVazio, novaInstanciaTarefaResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Erro_Quando_Descricao_Esta_Vazia()
    {
        // Arrange
        var tarefaId = new TarefaId(Guid.NewGuid());
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Nova Tarefa";
        var descricao = "";
        var dataInicio = DateTime.UtcNow;
        var dataFim = DateTime.UtcNow.AddDays(10);
        var status = StatusTarefa.Pendente;
        var prioridade = PrioridadeTarefa.Baixa;

        // Act
        var novaInstanciaTarefaResult = Tarefa.Criar(tarefaId, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade);

        // Assert
        Assert.Null(novaInstanciaTarefaResult.Value);
        Assert.True(novaInstanciaTarefaResult.IsFailure);
        Assert.Equal(TarefaErrors.DescricaoTarefaNaoPodeSerVazia, novaInstanciaTarefaResult.Error);
    }

    [Fact]
    public void Deve_Retornar_Error_DataInicio_For_Maior_Ou_Igual_DataFim()
    {
        // Arrange
        var tarefaId = new TarefaId(Guid.NewGuid());
        var projetoId = new ProjetoId(Guid.NewGuid());
        var usuarioId = new UsuarioId(Guid.NewGuid());
        var nome = "Nova Tarefa";
        var descricao = "Descrição da tarefa";
        var dataInicio = DateTime.UtcNow;
        var dataFim = dataInicio;
        var status = StatusTarefa.Pendente;
        var prioridade = PrioridadeTarefa.Baixa;

        // Act
        var novaInstanciaTarefaResult = Tarefa.Criar(tarefaId, projetoId, usuarioId, nome, descricao, dataInicio, dataFim, status, prioridade);

        // Assert
        Assert.Null(novaInstanciaTarefaResult.Value);
        Assert.True(novaInstanciaTarefaResult.IsFailure);
        Assert.Equal(TarefaErrors.DataInicioDeveSerMenorQueDataFim, novaInstanciaTarefaResult.Error);
    }
}
