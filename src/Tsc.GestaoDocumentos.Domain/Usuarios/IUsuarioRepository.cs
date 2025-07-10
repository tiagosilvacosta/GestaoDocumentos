using Tsc.GestaoDocumentos.Domain.Repositories;

namespace Tsc.GestaoDocumentos.Domain.Usuarios;

public interface IUsuarioRepository : IRepositorioComOrganizacao<Usuario, IdUsuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email, Guid tenantId, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorLoginAsync(string login, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> EmailExisteAsync(string email, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> EmailExisteAsync(string email, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default);
    Task<bool> LoginExisteAsync(string login, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> LoginExisteAsync(string login, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Usuario>> ObterPorPerfilAsync(int perfil, Guid tenantId, CancellationToken cancellationToken = default);
}
