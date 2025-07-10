using DddBase.Repositorio;

namespace Tsc.GestaoDocumentos.Domain.Organizacoes;

/// <summary>
/// Interface para repositório de Tenants.
/// </summary>
public interface IRepositorioOrganizacao : IRepositorio<Organizacao, IdOrganizacao>
{
    /// <summary>
    /// Obtém um tenant pelo slug.
    /// </summary>
    /// <param name="slug">Slug do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Tenant encontrado ou null</returns>
    Task<Organizacao?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default);
    
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
    Task<bool> SlugExisteAsync(string slug, IdOrganizacao excluirId, CancellationToken cancellationToken = default);
}
