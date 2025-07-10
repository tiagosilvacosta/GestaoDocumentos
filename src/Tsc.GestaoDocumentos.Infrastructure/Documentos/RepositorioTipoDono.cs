using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Documentos;

public class RepositorioTipoDono : RepositorioBaseComOrganizacao<TipoDono, IdTipoDono>, IRepositorioTipoDono
{
    public RepositorioTipoDono(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.IdOrganizacao == idOrganizacao, cancellationToken);
    }

    public async Task<bool> NomeExisteAsync(string nome, IdOrganizacao idOrganizacao, IdTipoDono excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.IdOrganizacao == idOrganizacao && td.Id != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<TipoDono>> ObterComTiposDocumentoAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(td => td.TiposDocumentoVinculados)
                .ThenInclude(tdtd => tdtd.TipoDocumento)
            .Where(td => td.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }
}
