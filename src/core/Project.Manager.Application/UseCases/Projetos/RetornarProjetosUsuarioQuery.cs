using Project.Manager.Application.Abstractions;

namespace Project.Manager.Application.UseCases.Projetos;

public sealed record RetornarProjetosUsuarioQuery(Guid UsuarioId) : IQuery;