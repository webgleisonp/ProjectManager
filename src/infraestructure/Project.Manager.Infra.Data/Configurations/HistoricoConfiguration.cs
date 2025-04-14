using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Configurations;

internal class HistoricoConfiguration : IEntityTypeConfiguration<Historico>
{
    public void Configure(EntityTypeBuilder<Historico> builder)
    {
        builder.ToTable("Historico");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new HistoricoId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.TarefaId)
            .HasConversion(
                id => id.Value,
                value => new TarefaId(value));

        builder.Property(x => x.UsuarioId)
            .HasConversion(
                id => id.Value,
                value => new UsuarioId(value));

        builder.Property(x => x.Tipo)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(x => x.Descricao)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(x => x.DataCriacao)
            .IsRequired();

        builder.HasOne(x => x.Tarefa)
            .WithMany(x => x.HistoricoEventos)
            .HasForeignKey(x => x.TarefaId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
