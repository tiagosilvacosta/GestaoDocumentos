using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class DocumentoRepository : RepositorioBaseComOrganizacao<Documento, IdDocumento>, IDocumentoRepository
{
    public DocumentoRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Documento>> ObterPorTipoDocumentoAsync(IdTipoDocumento idTipoDocumento, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.IdTipoDocumento == idTipoDocumento && d.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Documento>> ObterPorDonoDocumentoAsync(IdDonoDocumento idDonoDocumento, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.DonosVinculados.Any(dv => dv.IdDonoDocumento == idDonoDocumento) && d.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Documento>> ObterAtivosAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.EstaAtivo() && d.IdOrganizacao == idOrganizacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Documento>> ObterVersoesPorChaveAsync(string chaveBase, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.ChaveArmazenamento.StartsWith(chaveBase) && d.IdOrganizacao == idOrganizacao)
            .OrderByDescending(d => d.Versao)
            .ToListAsync(cancellationToken);
    }

    public async Task<Documento?> ObterVersaoAtivaAsync(IdTipoDocumento idTipoDocumento, IdDonoDocumento idDonoDocumento, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.IdTipoDocumento == idTipoDocumento &&
                       d.DonosVinculados.Any(dv => dv.IdDonoDocumento == idDonoDocumento) &&
                       d.EstaAtivo() &&
                       d.IdOrganizacao == idOrganizacao)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> ObterProximaVersaoAsync(string chaveBase, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        var ultimaVersao = await _dbSet
            .Where(d => d.ChaveArmazenamento.StartsWith(chaveBase) && d.IdOrganizacao == idOrganizacao)
            .MaxAsync(d => (int?)d.Versao, cancellationToken);

        return (ultimaVersao ?? 0) + 1;
    }

    public async Task<bool> ChaveArmazenamentoExisteAsync(string chaveArmazenamento, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(d => d.ChaveArmazenamento == chaveArmazenamento, cancellationToken);
    }

    public override async Task<Documento?> ObterPorIdAsync(IdDocumento id, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
                    .ThenInclude(dd => dd.TipoDono)
            .Where(d => d.Id.Valor == id.Valor)
            .FirstOrDefaultAsync(cancellationToken);
    }
}
