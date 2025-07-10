using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface ITipoDonoRepository : ITenantRepository<TipoDono>
{
    Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, Guid excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDono>> ObterComTiposDocumentoAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
