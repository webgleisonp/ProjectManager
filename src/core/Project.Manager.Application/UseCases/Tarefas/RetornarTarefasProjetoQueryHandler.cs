using Project.Manager.Application.Abstractions;
using Project.Manager.Application.Extensions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Domain.Errors;
using Project.Manager.Domain.Shared;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Application.UseCases.Tarefas;

internal sealed class RetornarTarefasProjetoQueryHandler(IProjetoRepository projetoRepository, ITarefaRepository tarefaRepository) : IQueryHandler<RetornarTarefasProjetoQuery, RetornarProjetoResponse>
{
    public async ValueTask<Result<RetornarProjetoResponse>> HandleAsync(RetornarTarefasProjetoQuery query, CancellationToken cancellationToken = default)
    {
        var projeto = await projetoRepository.RetornarProjetoAsync(new ProjetoId(query.ProjetoId), cancellationToken);

        if (projeto is null)
            return Result.Failure<RetornarProjetoResponse>(ProjetoErrors.ProjetoNaoEncontrado);

        var tarefas = await tarefaRepository.RetornaTarefasPorProjetoAsync(new ProjetoId(query.ProjetoId), cancellationToken);

        if (!tarefas.Any())
            return Result.Failure<RetornarProjetoResponse>(TarefaErrors.NenhumaTarefaEncontradaParaProjetoInformado);

        var tarefasResponse = tarefas.Select(t => t.ToRetornaTarefasProjetoResponse());

        return Result.Success(projeto.ToRetornarProjetoResponse(tarefasResponse));
    }
}