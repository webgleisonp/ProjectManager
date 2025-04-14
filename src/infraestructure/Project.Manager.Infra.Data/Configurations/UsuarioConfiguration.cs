using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Project.Manager.Domain.Entities;
using Project.Manager.Domain.ValueObjects.Identities;

namespace Project.Manager.Infra.Data.Configurations;

internal class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                id => id.Value,
                value => new UsuarioId(value))
            .ValueGeneratedNever();

        builder.Property(u => u.Nome)
            .IsRequired()
            .HasMaxLength(100);

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(e => e.Value)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(150);
        });

        builder.OwnsOne(u => u.Senha, senha =>
        {
            senha.Property(s => s.Value)
                .HasColumnName("SenhaHash")
                .IsRequired()
                .HasMaxLength(10);
        });

        builder.Property(u => u.Ativo)
            .IsRequired();

        builder.HasMany(u => u.Projetos)
               .WithOne(p => p.Usuario)
               .HasForeignKey(p => p.UsuarioId)
               .OnDelete(DeleteBehavior.NoAction);

        builder.HasMany(u => u.Historico)
                .WithOne(h => h.Usuario)
                .HasForeignKey(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.NoAction);
    }
}