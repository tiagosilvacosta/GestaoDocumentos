using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;

namespace Tsc.GestaoDocumentos.Infrastructure.Data.Configurations;

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

public class DocumentoConfiguration : IEntityTypeConfiguration<Documento>
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

public class LogAuditoriaConfiguration : IEntityTypeConfiguration<LogAuditoria>
{
    public void Configure(EntityTypeBuilder<LogAuditoria> builder)
    {
        builder.ToTable("LogsAuditoria");

        builder.HasKey(la => la.Id);

        builder.Property(la => la.Id)
            .ValueGeneratedNever();

        builder.Property(la => la.IdOrganizacao)
            .IsRequired();

        builder.Property(la => la.IdUsuario)
            .IsRequired();

        builder.Property(la => la.EntidadeAfetada)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(la => la.IdEntidade)
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
        builder.HasIndex(la => new { la.IdOrganizacao, la.IdUsuario })
            .HasDatabaseName("IX_LogsAuditoria_IdOrganizacao_IdUsuario");

        builder.HasIndex(la => new { la.IdOrganizacao, la.EntidadeAfetada, la.IdEntidade })
            .HasDatabaseName("IX_LogsAuditoria_IdOrganizacao_EntidadeAfetada_EntidadeId");

        builder.HasIndex(la => new { la.IdOrganizacao, la.Operacao })
            .HasDatabaseName("IX_LogsAuditoria_IdOrganizacao_Operacao");

        builder.HasIndex(la => new { la.IdOrganizacao, la.DataHoraOperacao })
            .HasDatabaseName("IX_LogsAuditoria_IdOrganizacao_DataHoraOperacao");

        // Relacionamentos
        builder.HasOne(la => la.Tenant)
            .WithMany()
            .HasForeignKey(la => la.IdOrganizacao)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(la => la.Usuario)
            .WithMany()
            .HasForeignKey(la => la.IdUsuario)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
