using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Documentos;

public class RepositorioTipoDocumento : RepositorioBaseComOrganizacao<TipoDocumento, IdTipoDocumento>, IRepositorioTipoDocumento
{
    public RepositorioTipoDocumento(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.IdOrganizacao == idOrganizacao, cancellationToken);
    }

    public async Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, IdTipoDocumento excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.IdOrganizacao == idOrganizacao && td.Id != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<TipoDocumento>> ObterComTiposDonoAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(td => td.TiposDonoVinculados)
                .ThenInclude(tdtd => tdtd.TipoDono)
            .Where(td => td.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TipoDocumento>> ObterPorTipoDonoAsync(IdTipoDono idTipoDono, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(td => td.TiposDonoVinculados)
                .ThenInclude(tdtd => tdtd.TipoDono)
            .Where(td => td.TiposDonoVinculados.Any(tdtd => tdtd.IdTipoDono == idTipoDono) && td.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }
}
