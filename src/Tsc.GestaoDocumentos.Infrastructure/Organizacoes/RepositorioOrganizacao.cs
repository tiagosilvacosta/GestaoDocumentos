using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Organizacoes;

/// <summary>
/// Implementação do repositório de Tenants.
/// </summary>
public class RepositorioOrganizacao : RepositorioBase<Organizacao, IdOrganizacao>, IRepositorioOrganizacao
{
    public RepositorioOrganizacao(GestaoDocumentosDbContext context) : base(context)
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
            .AnyAsync(t => t.Slug ==slug.ToLowerInvariant() && t.Id.Valor != excluirId.Valor, cancellationToken);
    }
}
