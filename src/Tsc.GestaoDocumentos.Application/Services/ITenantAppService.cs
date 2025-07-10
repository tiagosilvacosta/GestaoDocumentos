using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Interface do serviço de aplicação para gerenciamento de Tenants.
/// Responsável por orquestrar operações relacionadas a Tenants.
/// </summary>
public interface ITenantAppService
{
    /// <summary>
    /// Obtém um tenant por ID.
    /// </summary>
    /// <param name="id">ID do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tenant ou null se não encontrado</returns>
    Task<TenantDto?> ObterPorIdAsync(IdOrganizacao id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém um tenant por slug.
    /// </summary>
    /// <param name="slug">Slug do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tenant ou null se não encontrado</returns>
    Task<TenantDto?> ObterPorSlugAsync(string slug, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os tenants com paginação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de tenants</returns>
    Task<PagedResult<TenantDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo tenant.
    /// </summary>
    /// <param name="createTenant">Dados para criação do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tenant criado</returns>
    Task<TenantDto> CriarAsync(CreateTenantDto createTenant, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um tenant existente.
    /// </summary>
    /// <param name="id">ID do tenant a ser atualizado</param>
    /// <param name="updateTenant">Dados para atualização do tenant</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tenant atualizado</returns>
    Task<TenantDto> AtualizarAsync(IdOrganizacao id, UpdateTenantDto updateTenant, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um tenant.
    /// </summary>
    /// <param name="id">ID do tenant a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, false se não encontrado</returns>
    Task<bool> RemoverAsync(IdOrganizacao id, CancellationToken cancellationToken = default);
}
