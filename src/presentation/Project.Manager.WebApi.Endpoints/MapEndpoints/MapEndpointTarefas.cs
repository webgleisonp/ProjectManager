using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Domain.Errors;
using Project.Manager.WebApi.Endpoints.Abastractions;

namespace Project.Manager.WebApi.Endpoints.MapEndpoints;

internal sealed class MapEndpointTarefas : IEndpointMap
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("tarefas", IncluirTarefaProjetoAsync)
            .Produces<IncluirTarefaProjetoResponse>(StatusCodes.Status201Created)
            .Produces<IncluirTarefaProjetoResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Tarefas")
            .MapToApiVersion(1);

        app.MapPut("tarefas/{id}", AtualizarTarefaProjetoAsync)
            .Produces<AtualizarTarefaProjetoResponse>(StatusCodes.Status200OK)
            .Produces(404)
            .Produces<AtualizarTarefaProjetoResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Tarefas")
            .MapToApiVersion(1);

        app.MapDelete("tarefas/{id}", RemoverTarefaProjetoAsync)
            .Produces(StatusCodes.Status204NoContent)
            .Produces(404)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Tarefas")
            .MapToApiVersion(1);
    }

    public async Task<IResult> IncluirTarefaProjetoAsync(
        [FromBody] IncluirTarefaProjetoRequest request,
        [FromServices] ICommandHandler<IncluirTarefaProjetoCommand, IncluirTarefaProjetoResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new IncluirTarefaProjetoCommand(request.ProjetoId,
            request.Nome,
            request.Descricao,
            request.DataInicio.ToDateTime(TimeOnly.MinValue),
            request.DataFim.ToDateTime(TimeOnly.MinValue),
            request.Prioridade);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsSuccess)
            return Results.Created("/tarefas", result.Value);

        return Results.BadRequest(result.Error);
    }

    public async Task<IResult> AtualizarTarefaProjetoAsync(
        [FromRoute] Guid id,
        [FromBody] AtualizarTarefaProjetoRequest request,
        [FromServices] ICommandHandler<AtualizarTarefaProjetoCommand, AtualizarTarefaProjetoResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new AtualizarTarefaProjetoCommand(id,
            request.Nome,
            request.Descricao,
            request.DataInicio.ToDateTime(TimeOnly.MinValue),
            request.DataFim.ToDateTime(TimeOnly.MinValue),
            request.Status);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure && result.Error == TarefaErrors.TarefaNaoEncontrada)
            return Results.NotFound(result.Error);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.Error);
    }

    public async Task<IResult> RemoverTarefaProjetoAsync(
        [FromRoute] Guid id,
        [FromServices] ICommandHandler<RemoverTarefaProjetoCommand> handler,
        CancellationToken cancellationToken)
    {
        var command = new RemoverTarefaProjetoCommand(id);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsFailure && result.Error == TarefaErrors.TarefaNaoEncontrada)
            return Results.NotFound(result.Error);

        if (result.IsSuccess)
            return Results.NoContent();

        return Results.BadRequest(result.Error);
    }
}
