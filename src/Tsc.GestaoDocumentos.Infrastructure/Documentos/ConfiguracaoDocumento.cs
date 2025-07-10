using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Documentos;

namespace Tsc.GestaoDocumentos.Infrastructure.Documentos;

public class DonoDocumentoConfiguration : IEntityTypeConfiguration<DonoDocumento>
{
    public void Configure(EntityTypeBuilder<DonoDocumento> builder)
    {
        builder.ToTable("DonosDocumento");

        builder.HasKey(dd => dd.Id);

        builder.Property(dd => dd.Id)
            .ValueGeneratedNever();

        builder.Property(dd => dd.IdOrganizacao)
            .IsRequired();

        builder.Property(dd => dd.NomeAmigavel)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(dd => dd.IdTipoDono)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(dd => new { dd.IdOrganizacao, dd.NomeAmigavel })
            .HasDatabaseName("IX_DonosDocumento_IdOrganizacao_NomeAmigavel");

        builder.HasIndex(dd => new { dd.IdOrganizacao, dd.IdTipoDono })
            .HasDatabaseName("IX_DonosDocumento_IdOrganizacao_IdTipoDono");

        // Relacionamentos
        builder.HasOne(dd => dd.Tenant)
            .WithMany(t => t.DonosDocumento)
            .HasForeignKey(dd => dd.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dd => dd.TipoDono)
            .WithMany(td => td.DonosDocumento)
            .HasForeignKey(dd => dd.IdTipoDono)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class ConfiguracaoDocumento : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.ToTable("Documentos");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever();

        builder.Property(d => d.IdOrganizacao)
            .IsRequired();

        builder.Property(d => d.NomeArquivo)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(d => d.ChaveArmazenamento)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(d => d.DataUpload)
            .IsRequired();

        builder.Property(d => d.TamanhoArquivo)
            .IsRequired();

        builder.Property(d => d.TipoArquivo)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(d => d.Versao)
            .IsRequired();

        builder.Property(d => d.Status)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(d => d.IdTipoDocumento)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(d => new { d.IdOrganizacao, d.ChaveArmazenamento })
            .IsUnique()
            .HasDatabaseName("IX_Documentos_IdOrganizacao_ChaveArmazenamento");

        builder.HasIndex(d => new { d.IdOrganizacao, d.IdTipoDocumento })
            .HasDatabaseName("IX_Documentos_IdOrganizacao_IdTipoDocumento");

        builder.HasIndex(d => new { d.IdOrganizacao, d.Status })
            .HasDatabaseName("IX_Documentos_IdOrganizacao_Status");

        builder.HasIndex(d => new { d.IdOrganizacao, d.DataUpload })
            .HasDatabaseName("IX_Documentos_IdOrganizacao_DataUpload");

        // Relacionamentos
        builder.HasOne(d => d.Tenant)
            .WithMany()
            .HasForeignKey(d => d.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.TipoDocumento)
            .WithMany(td => td.Documentos)
            .HasForeignKey(d => d.IdTipoDocumento)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DocumentoDonoDocumentoConfiguration : IEntityTypeConfiguration<DocumentoDonoDocumento>
{
    public void Configure(EntityTypeBuilder<DocumentoDonoDocumento> builder)
    {
        builder.ToTable("DocumentoDonoDocumento");

        builder.HasKey(ddd => ddd.Id);

        builder.Property(ddd => ddd.Id)
            .ValueGeneratedNever();

        builder.Property(ddd => ddd.IdOrganizacao)
            .IsRequired();

        builder.Property(ddd => ddd.IdDocumento)
            .IsRequired();

        builder.Property(ddd => ddd.IdDonoDocumento)
            .IsRequired();

        // Índice único composto
        builder.HasIndex(ddd => new { ddd.IdOrganizacao, ddd.IdDocumento, ddd.IdDonoDocumento })
            .IsUnique()
            .HasDatabaseName("IX_DocumentoDonoDocumento_IdOrganizacao_IdDocumento_IdDonoDocumento");

        // Relacionamentos
        builder.HasOne(ddd => ddd.Documento)
            .WithMany(d => d.DonosVinculados)
            .HasForeignKey(ddd => ddd.IdDocumento)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ddd => ddd.DonoDocumento)
            .WithMany(dd => dd.DocumentosVinculados)
            .HasForeignKey(ddd => ddd.IdDonoDocumento)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
