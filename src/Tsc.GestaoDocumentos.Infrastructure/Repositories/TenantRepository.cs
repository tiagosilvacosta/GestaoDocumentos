using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

/// <summary>
/// Implementação do repositório de Tenants.
/// </summary>
public class TenantRepository : BaseRepository<Organizacao, IdOrganizacao>, ITenantRepository
{
    public TenantRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<Organizacao?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default)
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

    public async Task<bool> SlugExisteAsync(string slug, IdOrganizacao excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(t => t.Slug == slug.ToLowerInvariant() && t.Id.Valor != excluirId.Valor, cancellationToken);
    }
}
