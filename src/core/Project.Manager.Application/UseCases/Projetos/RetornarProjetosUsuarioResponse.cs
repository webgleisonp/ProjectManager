namespace Project.Manager.Application.UseCases.Projetos;

public sealed record RetornarProjetosUsuarioResponse(Guid Id, string Nome, string Descricao, DateTime DataInicio, DateTime? DataTermino, string Usuario);