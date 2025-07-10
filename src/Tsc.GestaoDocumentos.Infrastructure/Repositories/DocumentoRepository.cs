using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Repositories;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class DocumentoRepository : TenantBaseRepository<Documento>, IDocumentoRepository
{
    public DocumentoRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Documento>> ObterPorTipoDocumentoAsync(Guid tipoDocumentoId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.TipoDocumentoId == tipoDocumentoId && d.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Documento>> ObterPorDonoDocumentoAsync(Guid donoDocumentoId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.DonosVinculados.Any(dv => dv.DonoDocumentoId == donoDocumentoId) && d.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Documento>> ObterAtivosAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.EstaAtivo() && d.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Documento>> ObterVersoesPorChaveAsync(string chaveBase, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.ChaveArmazenamento.StartsWith(chaveBase) && d.TenantId == tenantId)
            .OrderByDescending(d => d.Versao)
            .ToListAsync(cancellationToken);
    }

    public async Task<Documento?> ObterVersaoAtivaAsync(Guid tipoDocumentoId, Guid donoDocumentoId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(d => d.TipoDocumento)
            .Include(d => d.DonosVinculados)
                .ThenInclude(dv => dv.DonoDocumento)
            .Where(d => d.TipoDocumentoId == tipoDocumentoId &&
                       d.DonosVinculados.Any(dv => dv.DonoDocumentoId == donoDocumentoId) &&
                       d.EstaAtivo() &&
                       d.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<int> ObterProximaVersaoAsync(string chaveBase, Guid tenantId, CancellationToken cancellationToken = default)
    {
        var ultimaVersao = await _dbSet
            .Where(d => d.ChaveArmazenamento.StartsWith(chaveBase) && d.TenantId == tenantId)
            .MaxAsync(d => (int?)d.Versao, cancellationToken);

        return (ultimaVersao ?? 0) + 1;
    }

    public async Task<bool> ChaveArmazenamentoExisteAsync(string chaveArmazenamento, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(d => d.ChaveArmazenamento == chaveArmazenamento, cancellationToken);
    }

    public override async Task<Documento?> ObterPorIdAsync(EntityId id, CancellationToken cancellationToken = default)
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
