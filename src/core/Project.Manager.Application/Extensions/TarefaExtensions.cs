using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.Extensions;

public static class TarefaExtensions
{
    public static RetornarTarefasProjetoResponse ToRetornaTarefasProjetoResponse(this Tarefa tarefa)
    {
        return new RetornarTarefasProjetoResponse(
            tarefa.Id.Value,
            tarefa.Nome,
            tarefa.Descricao,
            tarefa.DataInicio,
            tarefa.DataFim);
    }

    public static IncluirTarefaProjetoResponse ToIncluirTarefaProjetoResponse(this Tarefa tarefa)
    {
        return new IncluirTarefaProjetoResponse(
            tarefa.Id.Value,
            tarefa.Projeto.Nome,
            tarefa.Nome,
            tarefa.Descricao,
            tarefa.DataInicio,
            tarefa.DataFim);
    }
}