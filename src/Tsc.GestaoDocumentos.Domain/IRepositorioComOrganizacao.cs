using DddBase.Base;
using DddBase.Repositorio;
using Tsc.GestaoDocumentos.Domain.Common;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Domain.Repositories;

/// <summary>
/// Interface para repositórios multi-tenant.
/// </summary>
/// <typeparam name="T">Tipo da entidade (deve ser AggregateRoot)</typeparam>
public interface IRepositorioComOrganizacao<T, TId> : IRepositorio<T, TId>
    where T : EntidadeBase<TId>, IRaizAgregado
    where TId : ObjetoDeValor
{
    /// <summary>
    /// Obtém uma entidade pelo ID e ID da organização.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="idOrganizacao">Identificador da organização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Entidade encontrada ou null</returns>
    Task<T?> ObterPorIdETOrganizacaoAsync(TId id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todas as entidades da organização.
    /// </summary>
    /// <param name="idOrganizacao">Identificador da organização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista de entidades do tenant</returns>
    Task<IEnumerable<T>> ObterPorTenanteAsync(IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);

    /// <summary>
    /// Verifica se uma entidade existe para uma organização específica.
    /// </summary>
    /// <param name="id">Identificador da entidade</param>
    /// <param name="idOrganizacao">Identificador da organização</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se existe, false caso contrário</returns>
    Task<bool> ExisteComTenanteAsync(TId id, IdOrganizacao idOrganizacao, CancellationToken cancellationToken = default);
}
