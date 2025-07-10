using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Usuarios;

public class ConfiguracaoUsuario : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        builder.ToTable("Usuarios");

        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion<IdUsuarioConverter>()
            .ValueGeneratedNever();

        builder.Property(u => u.IdOrganizacao)
            .HasConversion<IdOrganizacaoConverter>()
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


