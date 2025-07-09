using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Common.Interfaces;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Infrastructure.Data.Configurations;

namespace Tsc.GestaoDocumentos.Infrastructure.Data;

public class GestaoDocumentosDbContext : DbContext
{
    private readonly ITenantContext? _tenantContext;

    public GestaoDocumentosDbContext(DbContextOptions<GestaoDocumentosDbContext> options)
        : base(options)
    {
    }

    public GestaoDocumentosDbContext(
        DbContextOptions<GestaoDocumentosDbContext> options,
        ITenantContext tenantContext)
        : base(options)
    {
        _tenantContext = tenantContext;
    }

    public DbSet<Tenant> Tenants { get; set; }
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

        // Aplicar configurações
        modelBuilder.ApplyConfiguration(new TenantConfiguration());
        modelBuilder.ApplyConfiguration(new UsuarioConfiguration());
        modelBuilder.ApplyConfiguration(new TipoDonoConfiguration());
        modelBuilder.ApplyConfiguration(new TipoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new TipoDonoTipoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new DonoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new DocumentoDonoDocumentoConfiguration());
        modelBuilder.ApplyConfiguration(new LogAuditoriaConfiguration());

        // Filtros globais para multi-tenancy
        if (_tenantContext != null)
        {
            modelBuilder.Entity<Usuario>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<TipoDono>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<TipoDocumento>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<TipoDonoTipoDocumento>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<DonoDocumento>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<Documento>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<DocumentoDonoDocumento>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<LogAuditoria>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);
        }
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Auto-definir TenantId nas entidades
        if (_tenantContext != null)
        {
            var tenantEntries = ChangeTracker.Entries()
                .Where(e => e.Entity is Domain.Common.TenantEntity && e.State == EntityState.Added)
                .Select(e => e.Entity as Domain.Common.TenantEntity)
                .Where(e => e != null && e.TenantId == Guid.Empty);

            foreach (var entity in tenantEntries)
            {
                entity!.DefinirTenant(_tenantContext.TenantId);
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
