using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Documentos;

public interface ITipoDonoRepository : IRepositorioComOrganizacao<TipoDono, IdTipoDono>
{
    Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, IdTipoDono excluirId, CancellationToken cancellationToken = default);
    Task<IEnumerable<TipoDono>> ObterComTiposDocumentoAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
