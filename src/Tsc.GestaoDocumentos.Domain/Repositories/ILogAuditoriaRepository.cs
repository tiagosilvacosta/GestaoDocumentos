using Tsc.GestaoDocumentos.Domain.Entities;
using Tsc.GestaoDocumentos.Domain.Enums;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface ILogAuditoriaRepository : ITenantRepository<LogAuditoria>
{
    Task<IEnumerable<LogAuditoria>> ObterPorUsuarioAsync(Guid usuarioId, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogAuditoria>> ObterPorEntidadeAsync(string entidadeAfetada, Guid entidadeId, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogAuditoria>> ObterPorOperacaoAsync(TipoOperacaoAuditoria operacao, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogAuditoria>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
