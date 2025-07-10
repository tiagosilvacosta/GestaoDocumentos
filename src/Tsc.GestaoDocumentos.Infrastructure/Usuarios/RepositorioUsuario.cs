using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Usuarios;

public class RepositorioUsuario : RepositorioBaseComOrganizacao<Usuario, IdUsuario>, IRepositorioUsuario
{
    public RepositorioUsuario(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<Usuario?> ObterPorEmailAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && u.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Usuario?> ObterPorLoginAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => u.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) && u.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && u.IdOrganizacao == idOrganizacao, cancellationToken);
    }

    public async Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, IdUsuario excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Email.Equals(email, StringComparison.InvariantCultureIgnoreCase) && u.IdOrganizacao == idOrganizacao && u.Id != excluirId, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) && u.IdOrganizacao == idOrganizacao, cancellationToken);
    }

    public async Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, IdUsuario excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(u => u.Login.Equals(login, StringComparison.InvariantCultureIgnoreCase) && u.IdOrganizacao == idOrganizacao && u.Id != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<Usuario>> ObterPorPerfilAsync(int perfil, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Where(u => (int)u.Perfil == perfil && u.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }
}
