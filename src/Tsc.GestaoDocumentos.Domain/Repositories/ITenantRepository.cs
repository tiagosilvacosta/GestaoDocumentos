using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Entities;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

/// <summary>
/// Interface para repositório de Tenants.
/// </summary>
public interface ITenantRepository : IBaseRepository<Tenant>
{
    /// <summary>
    /// Obtém um tenant pelo slug.
    /// </summary>
    /// <param name="slug">Slug do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Tenant encontrado ou null</returns>
    Task<Tenant?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica se um slug já existe.
    /// </summary>
    /// <param name="slug">Slug a ser verificado</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existe, false caso contrário</returns>
    Task<bool> SlugExisteAsync(string slug, CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Verifica se um slug já existe, excluindo um ID específico.
    /// </summary>
    /// <param name="slug">Slug a ser verificado</param>
    /// <param name="excluirId">ID a ser excluído da verificação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existe, false caso contrário</returns>
    Task<bool> SlugExisteAsync(string slug, EntityId excluirId, CancellationToken cancellationToken = default);
}
