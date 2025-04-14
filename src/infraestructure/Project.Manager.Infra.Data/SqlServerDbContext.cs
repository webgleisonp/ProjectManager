using Microsoft.EntityFrameworkCore;
using Project.Manager.Domain.Entities;

namespace Project.Manager.Infra.Data;

public sealed class SqlServerDbContext: DbContext
{
    public SqlServerDbContext(DbContextOptions<SqlServerDbContext> options)
        : base(options)
    {
        
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Projeto> Projetos { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }
    public DbSet<Historico> Historico { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer()
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors();
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Aplica o filtro global para todas as entidades que implementam Entity, e muda o tipo de string e datetime
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            // Iterar sobre todas as propriedades
            foreach (var property in entityType.GetProperties())
            {
                // Configurar varchar para strings em vez de nvarchar
                if (property.ClrType == typeof(string))
                {
                    property.SetColumnType("varchar"); // Ou defina o tamanho que preferir
                }

                // Configurar datetime para DateTime em vez de datetime2
                if (property.ClrType == typeof(DateTime))
                {
                    property.SetColumnType("datetime");
                }

                if (property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("datetime");
                }
            }
        }
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SqlServerDbContext).Assembly);
    }
}
