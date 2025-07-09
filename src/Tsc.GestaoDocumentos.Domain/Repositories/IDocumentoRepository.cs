using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface IDocumentoRepository : ITenantRepository<Documento>
{
    Task<IEnumerable<Documento>> ObterPorTipoDocumentoAsync(Guid tipoDocumentoId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Documento>> ObterPorDonoDocumentoAsync(Guid donoDocumentoId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Documento>> ObterAtivosAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Documento>> ObterVersoesPorChaveAsync(string chaveBase, Guid tenantId, CancellationToken cancellationToken = default);
    Task<Documento?> ObterVersaoAtivaAsync(Guid tipoDocumentoId, Guid donoDocumentoId, Guid tenantId, CancellationToken cancellationToken = default);
    Task<int> ObterProximaVersaoAsync(string chaveBase, Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> ChaveArmazenamentoExisteAsync(string chaveArmazenamento, CancellationToken cancellationToken = default);
}
