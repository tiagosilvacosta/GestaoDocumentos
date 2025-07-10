using Microsoft.EntityFrameworkCore;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;
using Tsc.GestaoDocumentos.Infrastructure.Data;
using Tsc.GestaoDocumentos.Infrastructure.Repositories;

namespace Tsc.GestaoDocumentos.Infrastructure.Logs;

public class RepositorioLogAuditoria : RepositorioBaseComOrganizacao<LogAuditoria, IdLogAuditoria>, IRepositorioLogAuditoria
{
    public RepositorioLogAuditoria(GestaoDocumentosDbContext context) : base(context)
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
