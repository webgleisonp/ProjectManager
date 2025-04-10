using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.UseCases.Projetos;

internal sealed class RetornarProjetosUsuarioQueryHandler(IProjetoRepository projetoRepository) : IQueryHandler<RetornarProjetosUsuarioQuery, IEnumerable<RetornarProjetosUsuarioResponse>>
{
    public async ValueTask<Result<IEnumerable<RetornarProjetosUsuarioResponse>>> HandleAsync(RetornarProjetosUsuarioQuery query, CancellationToken cancellationToken = default)
    {
        var usuarioId = new UsuarioId(query.UsuarioId);

        var projetos = await projetoRepository.RetornarProjetosUsuarioAsync(usuarioId, cancellationToken);

        if (!projetos.Any())
            return Result.Failure<IEnumerable<RetornarProjetosUsuarioResponse>>(ProjetoErrors.NaoExistemProjetosCadastradosParaUsuarioInformado);

        var response = projetos.Select(p => p.ToRetornarProjetosUsuarioResponse());

        return Result.Success(response)!;
    }
}