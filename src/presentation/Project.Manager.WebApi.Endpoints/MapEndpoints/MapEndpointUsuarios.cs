using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Application.UseCases.Usuarios;
using Project.Manager.Domain.Errors;
using Project.Manager.WebApi.Endpoints.Abastractions;

namespace Project.Manager.WebApi.Endpoints.MapEndpoints;

internal sealed class MapEndpointUsuarios : IEndpointMap
{
    public void MapEndpoints(IEndpointRouteBuilder app)
    {
        app.MapPost("usuarios", IncluirUsuarioAsync)
            .Produces<IncluirUsuarioCommandResponse>(StatusCodes.Status201Created)
            .Produces<IncluirUsuarioCommandResponse>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Usuarios")
            .MapToApiVersion(1);

        app.MapGet("usuarios/{id}/projetos", RetornarProjetosAsync)
            .Produces<IEnumerable<RetornarProjetosUsuarioResponse>>()
            .Produces(404)
            .Produces<IEnumerable<RetornarProjetosUsuarioResponse>>(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError)
            .WithTags("Usuarios")
            .MapToApiVersion(1);
    }

    public async Task<IResult> IncluirUsuarioAsync(
        [FromBody] IncluirUsuarioCommandRequest request,
        [FromServices] ICommandHandler<IncluirUsuarioCommand, IncluirUsuarioCommandResponse> handler,
        CancellationToken cancellationToken)
    {
        var command = new IncluirUsuarioCommand(request.Nome,
            request.Email,
            request.Senha,
            request.ConfirmarSenha);

        var result = await handler.HandleAsync(command, cancellationToken);

        if (result.IsSuccess)
            return Results.Created("/usuarios", result.Value);

        return Results.BadRequest(result.Error);
    }

    public async Task<IResult> RetornarProjetosAsync(
        [FromRoute] Guid id,
        [FromServices] IQueryHandler<RetornarProjetosUsuarioQuery, IEnumerable<RetornarProjetosUsuarioResponse>> handler,
        CancellationToken cancellationToken)
    {
        var query = new RetornarProjetosUsuarioQuery(id);

        var result = await handler.HandleAsync(query, cancellationToken);

        if (result.IsFailure && result.Error == ProjetoErrors.NaoExistemProjetosCadastradosParaUsuarioInformado)
            return Results.NotFound(result.Error);

        if (result.IsSuccess)
            return Results.Ok(result.Value);

        return Results.BadRequest(result.Error);
    }
}
