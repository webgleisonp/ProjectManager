using Project.Manager.Domain.Entities;

namespace Project.Manager.Application.UseCases.Projetos;

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
}