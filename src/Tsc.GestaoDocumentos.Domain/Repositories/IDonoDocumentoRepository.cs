using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface IDonoDocumentoRepository : ITenantRepository<DonoDocumento>
{
    Task<IEnumerable<DonoDocumento>> ObterComDocumentosAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonoDocumento>> ObterPorTipoDonoAsync(Guid tipoDonoId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<DonoDocumento?> ObterComDocumentosCompletosAsync(Guid id, Guid tenantId, CancellationToken cancellationToken = default);
}
