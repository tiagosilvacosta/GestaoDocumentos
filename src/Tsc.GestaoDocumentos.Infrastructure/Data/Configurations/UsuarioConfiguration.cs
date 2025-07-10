using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure.Data.Configurations;

public class UsuarioConfiguration : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .ValueGeneratedNever();

        builder.Property(u => u.IdOrganizacao)
            .IsRequired();

        builder.Property(u => u.Nome)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Email)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(u => u.Login)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(u => u.SenhaHash)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(u => u.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(u => u.Perfil)
            .HasConversion<int>()
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(u => new { u.IdOrganizacao, u.Email })
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_TenantId_Email");

        builder.HasIndex(u => new { u.IdOrganizacao, u.Login })
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_TenantId_Login");

        builder.HasIndex(u => new { u.IdOrganizacao, u.Status })
            .HasDatabaseName("IX_Usuarios_TenantId_Status");

        builder.HasIndex(u => new { u.IdOrganizacao, u.Perfil })
            .HasDatabaseName("IX_Usuarios_TenantId_Perfil");

        // Relacionamento
        builder.HasOne(u => u.Organizacao)
            .WithMany(t => t.Usuarios)
            .HasForeignKey(u => u.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TipoDonoConfiguration : IEntityTypeConfiguration<TipoDono>
{
    public void Configure(EntityTypeBuilder<TipoDono> builder)
    {
        builder.ToTable("TiposDono");

        builder.HasKey(td => td.Id);

        builder.Property(td => td.Id)
            .ValueGeneratedNever();

        builder.Property(td => td.TenantId)
            .IsRequired();

        builder.Property(td => td.Nome)
            .HasMaxLength(255)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(td => new { td.TenantId, td.Nome })
            .IsUnique()
            .HasDatabaseName("IX_TiposDono_TenantId_Nome");

        // Relacionamento
        builder.HasOne(td => td.Tenant)
            .WithMany(t => t.TiposDono)
            .HasForeignKey(td => td.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(td => td.DonosDocumento)
            .WithOne(dd => dd.TipoDono)
            .HasForeignKey(dd => dd.TipoDonoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDocumento>
{
    public void Configure(EntityTypeBuilder<TipoDocumento> builder)
    {
        builder.ToTable("TiposDocumento");

        builder.HasKey(td => td.Id);

        builder.Property(td => td.Id)
            .ValueGeneratedNever();

        builder.Property(td => td.TenantId)
            .IsRequired();

        builder.Property(td => td.Nome)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(td => td.PermiteMultiplosDocumentos)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(td => new { td.TenantId, td.Nome })
            .IsUnique()
            .HasDatabaseName("IX_TiposDocumento_TenantId_Nome");

        // Relacionamento
        builder.HasOne(td => td.Tenant)
            .WithMany(t => t.TiposDocumento)
            .HasForeignKey(td => td.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(td => td.Documentos)
            .WithOne(d => d.TipoDocumento)
            .HasForeignKey(d => d.TipoDocumentoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TipoDonoTipoDocumentoConfiguration : IEntityTypeConfiguration<TipoDonoTipoDocumento>
{
    public void Configure(EntityTypeBuilder<TipoDonoTipoDocumento> builder)
    {
        builder.ToTable("TipoDonoTipoDocumento");

        builder.HasKey(tdtd => tdtd.Id);

        builder.Property(tdtd => tdtd.Id)
            .ValueGeneratedNever();

        builder.Property(tdtd => tdtd.TenantId)
            .IsRequired();

        builder.Property(tdtd => tdtd.TipoDonoId)
            .IsRequired();

        builder.Property(tdtd => tdtd.TipoDocumentoId)
            .IsRequired();

        // Índice único composto
        builder.HasIndex(tdtd => new { tdtd.TenantId, tdtd.TipoDonoId, tdtd.TipoDocumentoId })
            .IsUnique()
            .HasDatabaseName("IX_TipoDonoTipoDocumento_TenantId_TipoDonoId_TipoDocumentoId");

        // Relacionamentos
        builder.HasOne(tdtd => tdtd.TipoDono)
            .WithMany(td => td.TiposDocumentoVinculados)
            .HasForeignKey(tdtd => tdtd.TipoDonoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tdtd => tdtd.TipoDocumento)
            .WithMany(td => td.TiposDonoVinculados)
            .HasForeignKey(tdtd => tdtd.TipoDocumentoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
