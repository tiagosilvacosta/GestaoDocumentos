using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Repositories;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class TenantRepository : BaseRepository<Tenant>, ITenantRepository
{
    public TenantRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(t => t.Slug == slug.ToLowerInvariant())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> SlugExisteAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(t => t.Slug == slug.ToLowerInvariant(), cancellationToken);
    }

    public async Task<bool> SlugExisteAsync(string slug, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(t => t.Slug == slug.ToLowerInvariant() && t.Id != excluirId, cancellationToken);
    }
}
