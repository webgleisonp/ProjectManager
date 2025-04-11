using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.Extensions;

public static class ProjetoExtensions
{
    public static IncluirProjetoCommandResponse ToIncluirProjetoCommandResponse(this Projeto projeto)
    {
        return new IncluirProjetoCommandResponse(projeto.Id.Value, projeto.Nome, projeto.Descricao, projeto.DataInicio, projeto.DataFim);
    }

    public static RetornarProjetosUsuarioResponse ToRetornarProjetosUsuarioResponse(this Projeto projeto)
    {
        return new RetornarProjetosUsuarioResponse(projeto.Id.Value, projeto.Nome, projeto.Descricao, projeto.DataInicio, projeto.DataFim, projeto.Usuario.Nome);
    }

    public static RetornarProjetoResponse ToRetornarProjetoResponse(this Projeto projeto, IEnumerable<RetornarTarefasProjetoResponse> tarefas)
    {
        return new RetornarProjetoResponse(projeto.Id.Value, projeto.Nome, projeto.Descricao, projeto.DataInicio, projeto.DataFim, projeto.Usuario.Nome, tarefas);
    }
}