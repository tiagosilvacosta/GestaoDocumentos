using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Documentos;

public class RepositorioDonoDocumento(GestaoDocumentosDbContext context) : RepositorioBaseComOrganizacao<DonoDocumento, IdDonoDocumento>(context), IRepositorioDonoDocumento
{
    public async Task<IEnumerable<DonoDocumento>> ObterComDocumentosAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(dd => dd.TipoDono)
            .Include(dd => dd.DocumentosVinculados)
                .ThenInclude(dv => dv.Documento)
                    .ThenInclude(d => d.TipoDocumento)
            .Where(dd => dd.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonoDocumento>> ObterPorTipoDonoAsync(IdTipoDono idTipoDono, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(dd => dd.TipoDono)
            .Include(dd => dd.DocumentosVinculados)
                .ThenInclude(dv => dv.Documento)
                    .ThenInclude(d => d.TipoDocumento)
            .Where(dd => dd.IdTipoDono == idTipoDono && dd.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<DonoDocumento?> ObterComDocumentosCompletosAsync(IdDonoDocumento id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(dd => dd.TipoDono)
                .ThenInclude(td => td.TiposDocumentoVinculados)
                    .ThenInclude(tdtd => tdtd.TipoDocumento)
            .Include(dd => dd.DocumentosVinculados)
                .ThenInclude(dv => dv.Documento)
                    .ThenInclude(d => d.TipoDocumento)
            .Where(dd => dd.Id == id && dd.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
