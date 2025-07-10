using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Documentos;
using Tsc.GestaoDocumentos.Infrastructure.Logs;
using Tsc.GestaoDocumentos.Infrastructure.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Usuarios;

namespace Tsc.GestaoDocumentos.Infrastructure.Data;

public class GestaoDocumentosDbContext : DbContext
{
    private readonly IContextoOrganizacao? _tenantContext;

    public GestaoDocumentosDbContext(DbContextOptions<GestaoDocumentosDbContext> options)
        : base(options)
    {
    }

    public GestaoDocumentosDbContext(
        DbContextOptions<GestaoDocumentosDbContext> options,
        IContextoOrganizacao tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Organizacao> Tenants { get; set; }
    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<TipoDono> TiposDono { get; set; }
    public DbSet<TipoDocumento> TiposDocumento { get; set; }
    public DbSet<TipoDonoTipoDocumento> TipoDonoTipoDocumento { get; set; }
    public DbSet<DonoDocumento> DonosDocumento { get; set; }
    public DbSet<Documento> Documentos { get; set; }
    public DbSet<DocumentoDonoDocumento> DocumentoDonoDocumento { get; set; }
    public DbSet<LogAuditoria> LogsAuditoria { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Explicitly ignore value objects that should not be treated as entities
        modelBuilder.Ignore<IdUsuario>();
        modelBuilder.Ignore<IdOrganizacao>();
        modelBuilder.Ignore<IdDonoDocumento>();
        modelBuilder.Ignore<IdDocumento>();
        modelBuilder.Ignore<IdTipoDono>();
        modelBuilder.Ignore<IdTipoDocumento>();
        modelBuilder.Ignore<IdDocumentoDonoDocumento>();
        modelBuilder.Ignore<IdTipoDonoTipoDocumento>();
        modelBuilder.Ignore<IdLogAuditoria>();

        // Aplicar configurações
        modelBuilder.ApplyConfiguration(new ConfiguracaoOrganizacao());
        modelBuilder.ApplyConfiguration(new ConfiguracaoUsuario());
        modelBuilder.ApplyConfiguration(new TipoDonoConfiguration());
        modelBuilder.ApplyConfiguration(new TipoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new TipoDonoTipoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new DonoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoDocumento());
        modelBuilder.ApplyConfiguration(new DocumentoDonoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new ConfiguracaoLogAuditoria());

        // Filtros globais para multi-tenancy
        if (_tenantContext != null)
        {
            modelBuilder.Entity<Usuario>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<TipoDono>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<TipoDocumento>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<TipoDonoTipoDocumento>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<DonoDocumento>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<Documento>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<DocumentoDonoDocumento>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);

            modelBuilder.Entity<LogAuditoria>()
                .HasQueryFilter(e => e.IdOrganizacao == _tenantContext.IdOrganizacao);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto-definir TenantId nas entidades
        if (_tenantContext != null)
        {
            var tenantEntries = ChangeTracker.Entries()
                .Where(e => e.Entity is IEntidadeComOrganizacao && e.State == EntityState.Added)
                .Select(e => e.Entity as IEntidadeComOrganizacao)
                .Where(e => e != null && e.IdOrganizacao.Valor == Guid.Empty);

            foreach (var entity in tenantEntries)
            {
                entity!.AlterarOrganizacao(_tenantContext.IdOrganizacao);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
