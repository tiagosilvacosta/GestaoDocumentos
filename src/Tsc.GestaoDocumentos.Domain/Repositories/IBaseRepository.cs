using DddBase.Repositorio;
using Tsc.GestaoDocumentos.Domain.Common;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

/// <summary>
/// Interface base para repositórios, herda de IRepositorio do Tsc.DddBase.
/// </summary>
/// <typeparam name="T">Tipo da entidade (deve ser AggregateRoot)</typeparam>
public interface IBaseRepository<T> : IRepositorio<T, EntityId> where T : AggregateRoot
{
    // IRepositorio já fornece os métodos básicos, apenas adicionamos métodos extras se necessário
}

/// <summary>
/// Interface para repositórios multi-tenant.
/// </summary>
/// <typeparam name="T">Tipo da entidade (deve ser AggregateRoot)</typeparam>
public interface ITenantRepository<T> : IBaseRepository<T> where T : AggregateRoot
{
    /// <summary>
    /// Obtém uma entidade pelo ID e ID do tenant.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="tenantId">Identificador do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade encontrada ou null</returns>
    Task<T?> ObterPorIdETenanteAsync(EntityId id, EntityId tenantId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Obtém todas as entidades de um tenant.
    /// </summary>
    /// <param name="tenantId">Identificador do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de entidades do tenant</returns>
    Task<IEnumerable<T>> ObterPorTenanteAsync(EntityId tenantId, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica se uma entidade existe para um tenant específico.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="tenantId">Identificador do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existe, false caso contrário</returns>
    Task<bool> ExisteComTenanteAsync(EntityId id, EntityId tenantId, CancellationToken cancellationToken = default);
}
