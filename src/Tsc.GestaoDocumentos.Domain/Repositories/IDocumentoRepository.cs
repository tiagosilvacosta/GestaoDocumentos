using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface IDocumentoRepository : IRepositorioComOrganizacao<Documento, IdDocumento>
{
    Task<IEnumerable<Documento>> ObterPorTipoDocumentoAsync(IdTipoDocumento idTipoDocumento, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<Documento>> ObterPorDonoDocumentoAsync(IdDonoDocumento idDonoDocumento, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<Documento>> ObterAtivosAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<Documento>> ObterVersoesPorChaveAsync(string chaveBase, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<Documento?> ObterVersaoAtivaAsync(IdTipoDocumento idTipoDocumento, IdDonoDocumento idDonoDocumento, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<int> ObterProximaVersaoAsync(string chaveBase, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> ChaveArmazenamentoExisteAsync(string chaveArmazenamento, CancellationToken cancellationToken = default);
}
