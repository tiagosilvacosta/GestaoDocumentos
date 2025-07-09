using Tsc.GestaoDocumentos.Domain.Repositories;

namespace Tsc.GestaoDocumentos.Domain.Common;

public interface IUnitOfWork : IDisposable
{
    ITenantRepository Tenants { get; }
    IUsuarioRepository Usuarios { get; }
    ITipoDonoRepository TiposDono { get; }
    ITipoDocumentoRepository TiposDocumento { get; }
    IDonoDocumentoRepository DonosDocumento { get; }
    IDocumentoRepository Documentos { get; }
    ILogAuditoriaRepository LogsAuditoria { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
