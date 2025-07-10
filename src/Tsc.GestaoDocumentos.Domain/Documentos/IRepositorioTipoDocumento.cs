using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public interface IRepositorioTipoDocumento : IRepositorioComOrganizacao<TipoDocumento, IdTipoDocumento>
{
    Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, IdTipoDocumento excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDocumento>> ObterComTiposDonoAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDocumento>> ObterPorTipoDonoAsync(IdTipoDono idTipoDono, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
