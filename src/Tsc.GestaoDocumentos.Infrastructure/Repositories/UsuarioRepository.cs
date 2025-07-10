using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class UsuarioRepository : TenantBaseRepository<Usuario>, IUsuarioRepository
{
    public UsuarioRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Email == email.ToLowerInvariant() && u.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Usuario?> ObterPorLoginAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Login == login.ToLowerInvariant() && u.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.TenantId == tenantId, cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.TenantId == tenantId && u.Id.Valor != excluirId, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login == login.ToLowerInvariant() && u.TenantId == tenantId, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login == login.ToLowerInvariant() && u.TenantId == tenantId && u.Id.Valor != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> ObterPorPerfilAsync(int perfil, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => (int)u.Perfil == perfil && u.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }
}
