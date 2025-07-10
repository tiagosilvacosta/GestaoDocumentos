using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Usuarios;

public class RepositorioUsuario(GestaoDocumentosDbContext context) : RepositorioBaseComOrganizacao<Usuario, IdUsuario>(context), IRepositorioUsuario
{
    public async Task<Usuario?> ObterPorEmailAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Email == email.ToLowerInvariant() && u.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Usuario?> ObterPorLoginAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Login == login.ToLowerInvariant() && u.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.IdOrganizacao == idOrganizacao, cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, IdUsuario excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email == email.ToLowerInvariant() && u.IdOrganizacao == idOrganizacao && u.Id != excluirId, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login == login.ToLowerInvariant() && u.IdOrganizacao == idOrganizacao, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, IdUsuario excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login == login.ToLowerInvariant() && u.IdOrganizacao == idOrganizacao && u.Id != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> ObterPorPerfilAsync(int perfil, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => (int)u.Perfil == perfil && u.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }
}
