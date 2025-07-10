using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Repositories;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class UsuarioRepository : TenantBaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Email == email.ToLowerInvariant() && u.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Usuario?> ObterPorLoginAsync(string login, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Login == login.ToLowerInvariant() && u.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.TenantId == tenantId, cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.TenantId == tenantId && u.Id.Valor != excluirId, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login == login.ToLowerInvariant() && u.TenantId == tenantId, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login == login.ToLowerInvariant() && u.TenantId == tenantId && u.Id.Valor != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> ObterPorPerfilAsync(int perfil, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => (int)u.Perfil == perfil && u.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }
}
