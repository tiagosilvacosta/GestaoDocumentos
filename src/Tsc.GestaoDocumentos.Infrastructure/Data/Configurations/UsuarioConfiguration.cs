using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Documentos;
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
            .HasDatabaseName("IX_Usuarios_IdOrganizacao_Email");

        builder.HasIndex(u => new { u.IdOrganizacao, u.Login })
            .IsUnique()
            .HasDatabaseName("IX_Usuarios_IdOrganizacao_Login");

        builder.HasIndex(u => new { u.IdOrganizacao, u.Status })
            .HasDatabaseName("IX_Usuarios_IdOrganizacao_Status");

        builder.HasIndex(u => new { u.IdOrganizacao, u.Perfil })
            .HasDatabaseName("IX_Usuarios_IdOrganizacao_Perfil");

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

        builder.Property(td => td.IdOrganizacao)
            .IsRequired();

        builder.Property(td => td.Nome)
            .HasMaxLength(255)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(td => new { td.IdOrganizacao, td.Nome })
            .IsUnique()
            .HasDatabaseName("IX_TiposDono_IdOrganizacao_Nome");

        // Relacionamento
        builder.HasOne(td => td.Tenant)
            .WithMany(t => t.TiposDono)
            .HasForeignKey(td => td.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(td => td.DonosDocumento)
            .WithOne(dd => dd.TipoDono)
            .HasForeignKey(dd => dd.IdTipoDono)
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

        builder.Property(td => td.IdOrganizacao)
            .IsRequired();

        builder.Property(td => td.Nome)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(td => td.PermiteMultiplosDocumentos)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(td => new { td.IdOrganizacao, td.Nome })
            .IsUnique()
            .HasDatabaseName("IX_TiposDocumento_IdOrganizacao_Nome");

        // Relacionamento
        builder.HasOne(td => td.Tenant)
            .WithMany(t => t.TiposDocumento)
            .HasForeignKey(td => td.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(td => td.Documentos)
            .WithOne(d => d.TipoDocumento)
            .HasForeignKey(d => d.IdTipoDocumento)
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

        builder.Property(tdtd => tdtd.IdOrganizacao)
            .IsRequired();

        builder.Property(tdtd => tdtd.IdTipoDono)
            .IsRequired();

        builder.Property(tdtd => tdtd.IdTipoDocumento)
            .IsRequired();

        // Índice único composto
        builder.HasIndex(tdtd => new { tdtd.IdOrganizacao, tdtd.IdTipoDono, tdtd.IdTipoDocumento })
            .IsUnique()
            .HasDatabaseName("IX_TipoDonoTipoDocumento_IdOrganizacao_IdTipoDono_IdTipoDocumento");

        // Relacionamentos
        builder.HasOne(tdtd => tdtd.TipoDono)
            .WithMany(td => td.TiposDocumentoVinculados)
            .HasForeignKey(tdtd => tdtd.IdTipoDono)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(tdtd => tdtd.TipoDocumento)
            .WithMany(td => td.TiposDonoVinculados)
            .HasForeignKey(tdtd => tdtd.IdTipoDocumento)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
