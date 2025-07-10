using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface IDonoDocumentoRepository : IRepositorioComOrganizacao<DonoDocumento, IdDonoDocumento>
{
    Task<IEnumerable<DonoDocumento>> ObterComDocumentosAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonoDocumento>> ObterPorTipoDonoAsync(Guid tipoDonoId, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<DonoDocumento?> ObterComDocumentosCompletosAsync(Guid id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
