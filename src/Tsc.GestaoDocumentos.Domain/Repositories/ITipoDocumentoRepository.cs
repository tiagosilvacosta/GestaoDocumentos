using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface ITipoDocumentoRepository : ITenantRepository<TipoDocumento>
{
    Task<bool> NomeExisteAsync(string nome, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> NomeExisteAsync(string nome, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDocumento>> ObterComTiposDonoAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDocumento>> ObterPorTipoDonoAsync(Guid tipoDonoId, Guid tenantId, CancellationToken cancellationToken = default);
}
