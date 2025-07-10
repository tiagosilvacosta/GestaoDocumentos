using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Documentos;


public class DonoDocumentoConfiguration : IEntityTypeConfiguration<DonoDocumento>
{
    public void Configure(EntityTypeBuilder<DonoDocumento> builder)
    {
        builder.ToTable("DonosDocumento");

        builder.HasKey(dd => dd.Id);

        builder.Property(dd => dd.Id)
            .HasConversion<IdDonoDocumentoConverter>()            
            .ValueGeneratedNever();

        builder.Property(dd => dd.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
            .IsRequired();

        builder.Property(dd => dd.NomeAmigavel)
            .HasMaxLength(255)
            .IsRequired();

        builder.Property(dd => dd.IdTipoDono)
            .HasConversion<IdTipoDonoConverter>()
            .IsRequired();

        // Índices compostos para multi-tenancy
        builder.HasIndex(dd => new { dd.IdOrganizacao, dd.NomeAmigavel })
            .HasDatabaseName("IX_DonosDocumento_IdOrganizacao_NomeAmigavel");

        builder.HasIndex(dd => new { dd.IdOrganizacao, dd.IdTipoDono })
            .HasDatabaseName("IX_DonosDocumento_IdOrganizacao_IdTipoDono");

        // Relacionamentos
        builder.HasOne(dd => dd.Organizacao)
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
            .HasConversion<IdDocumentoConverter>()
            .ValueGeneratedNever();

        builder.Property(d => d.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
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
            .HasConversion<IdTipoDocumentoConverter>()
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
        builder.HasOne(d => d.Organizacao)
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
            .HasConversion<IdDocumentoDonoDocumentoConverter>()
            .ValueGeneratedNever();

        builder.Property(ddd => ddd.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
            .IsRequired();

        builder.Property(ddd => ddd.IdDocumento)
            .HasConversion<IdDocumentoConverter>()
            .IsRequired();

        builder.Property(ddd => ddd.IdDonoDocumento)
            .HasConversion<IdDonoDocumentoConverter>()
            .IsRequired();

        // Índice único composto
        builder.HasIndex(ddd => new { ddd.IdOrganizacao, ddd.IdDocumento, ddd.IdDonoDocumento })
            .IsUnique()
            .HasDatabaseName("IX_DocumentoDonoDocumento_IdOrganizacao_IdDocumento_IdDonoDocumento");

        // Relacionamentos - alterado para evitar múltiplos caminhos de cascata
        builder.HasOne(ddd => ddd.Documento)
            .WithMany(d => d.DonosVinculados)
            .HasForeignKey(ddd => ddd.IdDocumento)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(ddd => ddd.DonoDocumento)
            .WithMany(dd => dd.DocumentosVinculados)
            .HasForeignKey(ddd => ddd.IdDonoDocumento)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class TipoDonoConfiguration : IEntityTypeConfiguration<TipoDono>
{
    public void Configure(EntityTypeBuilder<TipoDono> builder)
    {
        builder.ToTable("TiposDono");

        builder.HasKey(td => td.Id);

        builder.Property(td => td.Id)
            .HasConversion<IdTipoDonoConverter>()
            .ValueGeneratedNever();

        builder.Property(td => td.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
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
            .HasConversion<IdTipoDocumentoConverter>()
            .ValueGeneratedNever();

        builder.Property(td => td.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
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
            .HasConversion<IdTipoDonoTipoDocumentoConverter>()
            .ValueGeneratedNever();

        builder.Property(tdtd => tdtd.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
            .IsRequired();

        builder.Property(tdtd => tdtd.IdTipoDono)
            .HasConversion<IdTipoDonoConverter>()
            .IsRequired();

        builder.Property(tdtd => tdtd.IdTipoDocumento)
            .HasConversion<IdTipoDocumentoConverter>()
            .IsRequired();

        // Índice único composto
        builder.HasIndex(tdtd => new { tdtd.IdOrganizacao, tdtd.IdTipoDono, tdtd.IdTipoDocumento })
            .IsUnique()
            .HasDatabaseName("IX_TipoDonoTipoDocumento_IdOrganizacao_IdTipoDono_IdTipoDocumento");

        // Relacionamentos - alterado para evitar múltiplos caminhos de cascata
        builder.HasOne(tdtd => tdtd.TipoDono)
            .WithMany(td => td.TiposDocumentoVinculados)
            .HasForeignKey(tdtd => tdtd.IdTipoDono)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(tdtd => tdtd.TipoDocumento)
            .WithMany(td => td.TiposDonoVinculados)
            .HasForeignKey(tdtd => tdtd.IdTipoDocumento)
            .OnDelete(DeleteBehavior.Restrict);
    }
}