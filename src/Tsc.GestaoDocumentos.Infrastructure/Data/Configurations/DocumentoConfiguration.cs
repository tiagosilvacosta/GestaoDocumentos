using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Infrastructure.Data.Configurations;

public class DonoDocumentoConfiguration : IEntityTypeConfiguration<DonoDocumento>
{
    public void Configure(EntityTypeBuilder<DonoDocumento> builder)
    {
        builder.ToTable("DonosDocumento");

        builder.HasKey(dd => dd.Id);

        builder.Property(dd => dd.Id)
            .ValueGeneratedNever();

        builder.Property(dd => dd.TenantId)
            .IsRequired();

        builder.Property(dd => dd.NomeAmigavel)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(dd => dd.TipoDonoId)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(dd => new { dd.TenantId, dd.NomeAmigavel })
            .HasDatabaseName("IX_DonosDocumento_TenantId_NomeAmigavel");

        builder.HasIndex(dd => new { dd.TenantId, dd.TipoDonoId })
            .HasDatabaseName("IX_DonosDocumento_TenantId_TipoDonoId");

        // Relacionamentos
        builder.HasOne(dd => dd.Tenant)
            .WithMany(t => t.DonosDocumento)
            .HasForeignKey(dd => dd.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(dd => dd.TipoDono)
            .WithMany(td => td.DonosDocumento)
            .HasForeignKey(dd => dd.TipoDonoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
{
    public void Configure(EntityTypeBuilder<Documento> builder)
    {
        builder.ToTable("Documentos");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Id)
            .ValueGeneratedNever();

        builder.Property(d => d.TenantId)
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

        builder.Property(d => d.TipoDocumentoId)
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(d => new { d.TenantId, d.ChaveArmazenamento })
            .IsUnique()
            .HasDatabaseName("IX_Documentos_TenantId_ChaveArmazenamento");

        builder.HasIndex(d => new { d.TenantId, d.TipoDocumentoId })
            .HasDatabaseName("IX_Documentos_TenantId_TipoDocumentoId");

        builder.HasIndex(d => new { d.TenantId, d.Status })
            .HasDatabaseName("IX_Documentos_TenantId_Status");

        builder.HasIndex(d => new { d.TenantId, d.DataUpload })
            .HasDatabaseName("IX_Documentos_TenantId_DataUpload");

        // Relacionamentos
        builder.HasOne(d => d.Tenant)
            .WithMany()
            .HasForeignKey(d => d.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(d => d.TipoDocumento)
            .WithMany(td => td.Documentos)
            .HasForeignKey(d => d.TipoDocumentoId)
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

        builder.Property(ddd => ddd.TenantId)
            .IsRequired();

        builder.Property(ddd => ddd.DocumentoId)
            .IsRequired();

        builder.Property(ddd => ddd.DonoDocumentoId)
            .IsRequired();

        // Índice único composto
        builder.HasIndex(ddd => new { ddd.TenantId, ddd.DocumentoId, ddd.DonoDocumentoId })
            .IsUnique()
            .HasDatabaseName("IX_DocumentoDonoDocumento_TenantId_DocumentoId_DonoDocumentoId");

        // Relacionamentos
        builder.HasOne(ddd => ddd.Documento)
            .WithMany(d => d.DonosVinculados)
            .HasForeignKey(ddd => ddd.DocumentoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(ddd => ddd.DonoDocumento)
            .WithMany(dd => dd.DocumentosVinculados)
            .HasForeignKey(ddd => ddd.DonoDocumentoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoria>
{
    public void Configure(EntityTypeBuilder<LogAuditoria> builder)
    {
        builder.ToTable("LogsAuditoria");

        builder.HasKey(la => la.Id);

        builder.Property(la => la.Id)
            .ValueGeneratedNever();

        builder.Property(la => la.TenantId)
            .IsRequired();

        builder.Property(la => la.UsuarioId)
            .IsRequired();

        builder.Property(la => la.EntidadeAfetada)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(la => la.EntidadeId)
            .IsRequired();

        builder.Property(la => la.Operacao)
            .HasConversion<int>()
            .IsRequired();

        builder.Property(la => la.DadosAnteriores)
            .HasColumnType("nvarchar(max)");

        builder.Property(la => la.DadosNovos)
            .HasColumnType("nvarchar(max)");

        builder.Property(la => la.DataHoraOperacao)
            .IsRequired();

        builder.Property(la => la.IpUsuario)
            .HasMaxLength(45)
            .IsRequired();

        builder.Property(la => la.UserAgent)
            .HasMaxLength(500);

        // Índices compostos para multi-tenancy
        builder.HasIndex(la => new { la.TenantId, la.UsuarioId })
            .HasDatabaseName("IX_LogsAuditoria_TenantId_UsuarioId");

        builder.HasIndex(la => new { la.TenantId, la.EntidadeAfetada, la.EntidadeId })
            .HasDatabaseName("IX_LogsAuditoria_TenantId_EntidadeAfetada_EntidadeId");

        builder.HasIndex(la => new { la.TenantId, la.Operacao })
            .HasDatabaseName("IX_LogsAuditoria_TenantId_Operacao");

        builder.HasIndex(la => new { la.TenantId, la.DataHoraOperacao })
            .HasDatabaseName("IX_LogsAuditoria_TenantId_DataHoraOperacao");

        // Relacionamentos
        builder.HasOne(la => la.Tenant)
            .WithMany()
            .HasForeignKey(la => la.TenantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(la => la.Usuario)
            .WithMany()
            .HasForeignKey(la => la.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
