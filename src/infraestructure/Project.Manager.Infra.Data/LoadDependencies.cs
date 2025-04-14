using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Project.Manager.Application.Abstractions;
using Project.Manager.Domain.Abstractions.Repositories;
using Project.Manager.Infra.Data.Repositories;

namespace Project.Manager.Infra.Data;

public static class LoadDependencies
{
    public static IServiceCollection AddInfraestructureData(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<SqlServerDbContext>(options =>
        {
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
        });

        services.AddScoped<IUsuarioRepository, UsuarioRepository>();
        services.AddScoped<IProjetoRepository, ProjetoRepository>();
        services.AddScoped<ITarefaRepository, TarefaRepository>();

        services.AddScoped<IUnityOfWork, UnityOfWork>();

        return services;
    }
}