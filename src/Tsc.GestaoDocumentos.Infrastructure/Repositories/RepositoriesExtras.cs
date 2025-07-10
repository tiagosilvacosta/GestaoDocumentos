using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;

namespace Tsc.GestaoDocumentos.Infrastructure.Repositories;

public class TipoDonoRepository : RepositorioBaseComOrganizacao<TipoDono, IdTipoDono>, ITipoDonoRepository
{
    public TipoDonoRepository(GestaoDocumentosDbContext context) : base(context)
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

public class TipoDocumentoRepository : RepositorioBaseComOrganizacao<TipoDocumento, IdTipoDocumento>, ITipoDocumentoRepository
{
    public TipoDocumentoRepository(GestaoDocumentosDbContext context) : base(context)
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

public class DonoDocumentoRepository : RepositorioBaseComOrganizacao<DonoDocumento, IdDonoDocumento>, IDonoDocumentoRepository
{
    public DonoDocumentoRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

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

public class LogAuditoriaRepository : RepositorioBaseComOrganizacao<LogAuditoria, IdLogAuditoria>, ILogAuditoriaRepository
{
    public LogAuditoriaRepository(GestaoDocumentosDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorUsuarioAsync(IdUsuario idUsuario, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.IdUsuario == idUsuario && la.IdOrganizacao == idOrganizacao)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorEntidadeAsync(string entidadeAfetada, Guid idEntidade, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.EntidadeAfetada == entidadeAfetada && la.IdEntidade == idEntidade && la.IdOrganizacao == idOrganizacao)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorOperacaoAsync(TipoOperacaoAuditoria operacao, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.Operacao == operacao && la.IdOrganizacao == idOrganizacao)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<LogAuditoria>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default)
    {
        return await _dbSet
            .Include(la => la.Usuario)
            .Where(la => la.DataHoraOperacao >= dataInicio && la.DataHoraOperacao <= dataFim && la.IdOrganizacao == idOrganizacao)
            .OrderByDescending(la => la.DataHoraOperacao)
            .ToListAsync(cancellationToken);
    }
}
