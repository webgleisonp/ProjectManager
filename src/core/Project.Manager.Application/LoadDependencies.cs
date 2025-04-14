using Microsoft.Extensions.DependencyInjection;
using Project.Manager.Application.Abstractions;
using Project.Manager.Application.UseCases.Projetos;
using Project.Manager.Application.UseCases.Tarefas;
using Project.Manager.Application.UseCases.Usuarios;

namespace Project.Manager.Application;

public static class LoadDependencies
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ICommandHandler<IncluirUsuarioCommand, IncluirUsuarioCommandResponse>, IncluirUsuarioCommandHandler>();
        services.AddScoped<IQueryHandler<RetornarProjetosUsuarioQuery, IEnumerable<RetornarProjetosUsuarioResponse>>, RetornarProjetosUsuarioQueryHandler >();
        services.AddScoped<ICommandHandler<IncluirProjetoCommand, IncluirProjetoCommandResponse>, IncluirProjetoCommandHandler>();
        services.AddScoped<IQueryHandler<RetornarTarefasProjetoQuery, RetornarProjetoResponse>, RetornarTarefasProjetoQueryHandler>();
        services.AddScoped<ICommandHandler<IncluirTarefaProjetoCommand, IncluirTarefaProjetoResponse>, IncluirTarefaProjetoCommandHandler>();
        services.AddScoped<ICommandHandler<AtualizarTarefaProjetoCommand, AtualizarTarefaProjetoResponse>, AtualizarTarefaProjetoCommandHandler>();
        services.AddScoped<ICommandHandler<RemoverTarefaProjetoCommand>, RemoverTarefaProjetoCommandHandler>();

        return services;
    }
}
