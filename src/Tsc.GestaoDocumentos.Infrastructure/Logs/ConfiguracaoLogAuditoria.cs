using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Logs;

public class ConfiguracaoLogAuditoria : IEntityTypeConfiguration<LogAuditoria>
{
    public void Configure(EntityTypeBuilder<LogAuditoria> builder)
    {
        builder.ToTable("LogsAuditoria");

        builder.HasKey(la => la.Id);

        builder.Property(la => la.Id)
            .HasConversion<IdLogAuditoriaConverter>()
            .ValueGeneratedNever();

        builder.Property(la => la.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
            .IsRequired();

        builder.Property(la => la.IdUsuario)
            .HasConversion<IdUsuarioConverter>()
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

        // Configurar corretamente o relacionamento, especificando que IdUsuario mapeia para Usuario.Id
        builder.HasOne(la => la.Usuario)
            .WithMany()
            .HasForeignKey(la => la.IdUsuario)
            .HasPrincipalKey(u => u.Id)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
