using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public interface IDonoDocumentoRepository : IRepositorioComOrganizacao<DonoDocumento, IdDonoDocumento>
{
    Task<IEnumerable<DonoDocumento>> ObterComDocumentosAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<DonoDocumento>> ObterPorTipoDonoAsync(IdTipoDono idTipoDono, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<DonoDocumento?> ObterComDocumentosCompletosAsync(IdDonoDocumento id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
