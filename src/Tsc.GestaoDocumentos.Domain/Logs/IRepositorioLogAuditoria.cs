using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Logs;

public interface IRepositorioLogAuditoria : IRepositorioComOrganizacao<LogAuditoria, IdLogAuditoria>
{
    Task<IEnumerable<LogAuditoria>> ObterPorUsuarioAsync(IdUsuario idUsuario, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogAuditoria>> ObterPorEntidadeAsync(string entidadeAfetada, Guid idEntidade, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogAuditoria>> ObterPorOperacaoAsync(TipoOperacaoAuditoria operacao, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
    Task<IEnumerable<LogAuditoria>> ObterPorPeriodoAsync(DateTime dataInicio, DateTime dataFim, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
