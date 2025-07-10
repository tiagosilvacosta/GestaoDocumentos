using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Usuarios;

public interface IRepositorioUsuario : IRepositorioComOrganizacao<Usuario, IdUsuario>
{
    Task<Usuario?> ObterPorEmailAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<Usuario?> ObterPorLoginAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> EmailExisteAsync(string email, IdOrganizacao idOrganizacao, IdUsuario excluirId, CancellationToken cancellationToken = default);
    Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> LoginExisteAsync(string login, IdOrganizacao idOrganizacao, IdUsuario excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Usuario>> ObterPorPerfilAsync(int perfil, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
