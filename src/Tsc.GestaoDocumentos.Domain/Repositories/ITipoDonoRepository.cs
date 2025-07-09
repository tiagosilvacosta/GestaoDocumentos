using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface ITipoDonoRepository : ITenantRepository<TipoDono>
{
    Task<bool> NomeExisteAsync(string nome, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> NomeExisteAsync(string nome, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDono>> ObterComTiposDocumentoAsync(Guid tenantId, CancellationToken cancellationToken = default);
}
