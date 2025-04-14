using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Configurations;

internal class TarefaConfiguration : IEntityTypeConfiguration<Tarefa>
{
    public void Configure(EntityTypeBuilder<Tarefa> builder)
    {
        builder.ToTable("Tarefas");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new TarefaId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.ProjetoId)
            .HasConversion(
                id => id.Value,
                value => new ProjetoId(value));

        builder.Property(x => x.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DataInicio)
            .IsRequired();

        builder.Property(x => x.DataFim)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.Prioridade)
            .HasConversion<int>()
            .IsRequired();

        builder.HasOne(t => t.Projeto)
               .WithMany(p => p.Tarefas)
               .HasForeignKey(t => t.ProjetoId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Historico)
            .WithOne(x => x.Tarefa)
            .HasForeignKey(x => x.TarefaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
