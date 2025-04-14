using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Configurations;

internal class ProjetoConfiguration : IEntityTypeConfiguration<Projeto>
{
    public void Configure(EntityTypeBuilder<Projeto> builder)
    {
        builder.ToTable("Projetos");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasConversion(
                id => id.Value,
                value => new ProjetoId(value))
            .ValueGeneratedNever();

        builder.Property(x => x.UsuarioId)
            .HasConversion(
                id => id.Value,
                value => new UsuarioId(value));

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

        builder.Property(x => x.Ativo)
            .IsRequired();

        builder.HasOne(x => x.Usuario)
            .WithMany(x => x.Projetos)
            .HasForeignKey(x => x.UsuarioId)
            .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(x => x.Tarefas)
            .WithOne(x => x.Projeto)
            .HasForeignKey(x => x.ProjetoId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
