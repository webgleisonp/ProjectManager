using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Errors;
using Project.Manager.WebApi.Endpoints.Abastractions;

namespace Project.Manager.WebApi.Endpoints.MapEndpoints;

internal sealed class MapEndpointProjetos : IEndpointMap
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapGet("projetos/{id}/tarefas", RetornarTarefasProjetoAsync)
            .Produces<RetornarProjetoResponse>()
            .Produces(404)
            .Produces<RetornarProjetoResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Projetos")
            .MapToApiVersion(1);

        app.MapPost("projetos", IncluirProjetoAsync)
            .Produces<IncluirProjetoCommandResponse>(StatusCodes.Status201Created)
            .Produces<IncluirProjetoCommandResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Projetos")
            .MapToApiVersion(1);
    }

    private static async Task<IResult> RetornarTarefasProjetoAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<RetornarTarefasProjetoQuery, RetornarProjetoResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new RetornarTarefasProjetoQuery(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure && result.Error == ProjetoErrors.ProjetoNaoEncontrado)
            return Results.NotFound(result.Error);

        if (result.IsFailure && result.Error == TarefaErrors.NenhumaTarefaEncontradaParaProjetoInformado)
            return Results.NotFound(result.Error);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.Error);
    }

    public async Task<IResult> IncluirProjetoAsync(
        [FromBody] IncluirProjetoCommandRequest request,
        [FromServices] ICommandHandler<IncluirProjetoCommand, IncluirProjetoCommandResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new IncluirProjetoCommand(request.UsuarioId,
            request.Nome,
            request.Descricao,
            request.DataInicio.ToDateTime(TimeOnly.MinValue),
            request.DataFim.ToDateTime(TimeOnly.MinValue));

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsSuccess)
            return Results.Created("/projetos", result.Value);

        return Results.BadRequest(result.Error);
    }
}
