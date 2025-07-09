using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

public interface IBaseRepository<T> where T : class
{
    Task<T?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ObterTodosAsync(CancellationToken cancellationToken = default);
    Task<T> AdicionarAsync(T entity, CancellationToken cancellationToken = default);
    Task AtualizarAsync(T entity, CancellationToken cancellationToken = default);
    Task RemoverAsync(T entity, CancellationToken cancellationToken = default);
    Task<bool> ExisteAsync(Guid id, CancellationToken cancellationToken = default);
}

public interface ITenantRepository<T> : IBaseRepository<T> where T : class
{
    Task<T?> ObterPorIdETenanteAsync(Guid id, Guid tenantId, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> ObterPorTenanteAsync(Guid tenantId, CancellationToken cancellationToken = default);
    Task<bool> ExisteComTenanteAsync(Guid id, Guid tenantId, CancellationToken cancellationToken = default);
}
