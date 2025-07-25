using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Organizacoes;

public class ConfiguracaoOrganizacao : IEntityTypeConfiguration<Organizacao>
{
    public void Configure(EntityTypeBuilder<Organizacao> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion<IdOrganizacaoConverter>()
            .ValueGeneratedNever();

        builder.Property(t => t.NomeOrganizacao)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(t => t.Slug)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(t => t.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(t => t.DataCriacao)
            .IsRequired();

        builder.Property(t => t.DataAtualizacao)
            .IsRequired();

        builder.Property(t => t.UsuarioCriacao)
            .HasConversion<IdUsuarioConverter>()
            .IsRequired();

        builder.Property(t => t.UsuarioUltimaAlteracao)
            .HasConversion<IdUsuarioConverter>()
            .IsRequired();

        // Índices
        builder.HasIndex(t => t.Slug)
            .IsUnique()
            .HasDatabaseName("IX_Tenants_Slug");

        builder.HasIndex(t => t.NomeOrganizacao)
            .HasDatabaseName("IX_Tenants_NomeOrganizacao");

        builder.HasIndex(t => t.Status)
            .HasDatabaseName("IX_Tenants_Status");

        // Relacionamentos
        builder.HasMany(t => t.Usuarios)
            .WithOne(u => u.Organizacao)
            .HasForeignKey(u => u.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TiposDono)
            .WithOne(td => td.Tenant)
            .HasForeignKey(td => td.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TiposDocumento)
            .WithOne(td => td.Tenant)
            .HasForeignKey(td => td.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.DonosDocumento)
            .WithOne(dd => dd.Organizacao)
            .HasForeignKey(dd => dd.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
