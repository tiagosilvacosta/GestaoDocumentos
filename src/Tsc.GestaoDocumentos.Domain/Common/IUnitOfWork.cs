using Tsc.GestaoDocumentos.Domain.Documentos;
using Tsc.GestaoDocumentos.Domain.Logs;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Domain.Common;

public interface IUnitOfWork : IDisposable
{
    IRepositorioOrganizacao Tenants { get; }
    IRepositorioUsuario Usuarios { get; }
    IRepositorioTipoDono TiposDono { get; }
    IRepositorioTipoDocumento TiposDocumento { get; }
    IRepositorioDonoDocumento DonosDocumento { get; }
    IRepositorioDocumento Documentos { get; }
    IRepositorioLogAuditoria LogsAuditoria { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
