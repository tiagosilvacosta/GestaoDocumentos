using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Infrastructure.Data.Configurations;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("Tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
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
            .IsRequired();

        builder.Property(t => t.UsuarioUltimaAlteracao)
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
            .WithOne(u => u.Tenant)
            .HasForeignKey(u => u.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TiposDono)
            .WithOne(td => td.Tenant)
            .HasForeignKey(td => td.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.TiposDocumento)
            .WithOne(td => td.Tenant)
            .HasForeignKey(td => td.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.DonosDocumento)
            .WithOne(dd => dd.Tenant)
            .HasForeignKey(dd => dd.TenantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
