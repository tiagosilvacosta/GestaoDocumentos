using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Repositories;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class TipoDonoRepository : TenantBaseRepository<TipoDono>, ITipoDonoRepository
{
    public TipoDonoRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<bool> NomeExisteAsync(string nome, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.TenantId == tenantId, cancellationToken);
    }

    public async Task<bool> NomeExisteAsync(string nome, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.TenantId == tenantId && td.Id != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<TipoDono>> ObterComTiposDocumentoAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(td => td.TiposDocumentoVinculados)
                .ThenInclude(tdtd => tdtd.TipoDocumento)
            .Where(td => td.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }
}

public class TipoDocumentoRepository : TenantBaseRepository<TipoDocumento>, ITipoDocumentoRepository
{
    public TipoDocumentoRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<bool> NomeExisteAsync(string nome, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.TenantId == tenantId, cancellationToken);
    }

    public async Task<bool> NomeExisteAsync(string nome, Guid tenantId, Guid excluirId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .AnyAsync(td => td.Nome == nome && td.TenantId == tenantId && td.Id != excluirId, cancellationToken);
    }

    public async Task<IEnumerable<TipoDocumento>> ObterComTiposDonoAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(td => td.TiposDonoVinculados)
                .ThenInclude(tdtd => tdtd.TipoDono)
            .Where(td => td.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<TipoDocumento>> ObterPorTipoDonoAsync(Guid tipoDonoId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(td => td.TiposDonoVinculados)
                .ThenInclude(tdtd => tdtd.TipoDono)
            .Where(td => td.TiposDonoVinculados.Any(tdtd => tdtd.TipoDonoId == tipoDonoId) && td.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }
}

public class DonoDocumentoRepository : TenantBaseRepository<DonoDocumento>, IDonoDocumentoRepository
{
    public DonoDocumentoRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<DonoDocumento>> ObterComDocumentosAsync(Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(dd => dd.TipoDono)
            .Include(dd => dd.DocumentosVinculados)
                .ThenInclude(dv => dv.Documento)
                    .ThenInclude(d => d.TipoDocumento)
            .Where(dd => dd.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<DonoDocumento>> ObterPorTipoDonoAsync(Guid tipoDonoId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(dd => dd.TipoDono)
            .Include(dd => dd.DocumentosVinculados)
                .ThenInclude(dv => dv.Documento)
                    .ThenInclude(d => d.TipoDocumento)
            .Where(dd => dd.TipoDonoId == tipoDonoId && dd.TenantId == tenantId)
            .ToListAsync(cancellationToken);
    }

    public async Task<DonoDocumento?> ObterComDocumentosCompletosAsync(Guid id, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(dd => dd.TipoDono)
                .ThenInclude(td => td.TiposDocumentoVinculados)
                    .ThenInclude(tdtd => tdtd.TipoDocumento)
            .Include(dd => dd.DocumentosVinculados)
                .ThenInclude(dv => dv.Documento)
                    .ThenInclude(d => d.TipoDocumento)
            .Where(dd => dd.Id == id && dd.TenantId == tenantId)
            .FirstOrDefaultAsync(cancellationToken);
    }
}

public class LogAuditoriaRepository : TenantBaseRepository<LogAuditoria>, ILogAuditoriaRepository
{
    public LogAuditoriaRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorUsuarioAsync(Guid usuarioId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.UsuarioId == usuarioId && la.TenantId == tenantId)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorEntidadeAsync(string entidadeAfetada, Guid entidadeId, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.EntidadeAfetada == entidadeAfetada && la.EntidadeId == entidadeId && la.TenantId == tenantId)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorOperacaoAsync(Domain.Enums.TipoOperacaoAuditoria operacao, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.Operacao == operacao && la.TenantId == tenantId)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, Guid tenantId, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.DataHoraOperacao >= dataInicio && la.DataHoraOperacao <= dataFim && la.TenantId == tenantId)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }
}
