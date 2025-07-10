using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Interface do serviço de aplicação para gerenciamento de Donos de Documento.
/// Responsável por orquestrar operações relacionadas a Donos de Documento.
/// </summary>
public interface IDonoDocumentoAppService
{
    /// <summary>
    /// Obtém um dono de documento por ID.
    /// </summary>
    /// <param name="id">ID do dono de documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do dono de documento ou null se não encontrado</returns>
    Task<DonoDocumentoDto?> ObterPorIdAsync(IdDonoDocumento id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os donos de documento do tenant atual com paginação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de donos de documento</returns>
    Task<PagedResult<DonoDocumentoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo dono de documento.
    /// </summary>
    /// <param name="createDonoDocumento">Dados para criação do dono de documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do dono de documento criado</returns>
    Task<DonoDocumentoDto> CriarAsync(CreateDonoDocumentoDto createDonoDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um dono de documento existente.
    /// </summary>
    /// <param name="id">ID do dono de documento a ser atualizado</param>
    /// <param name="updateDonoDocumento">Dados para atualização do dono de documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do dono de documento atualizado</returns>
    Task<DonoDocumentoDto> AtualizarAsync(IdDonoDocumento id, UpdateDonoDocumentoDto updateDonoDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um dono de documento.
    /// </summary>
    /// <param name="id">ID do dono de documento a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, false se não encontrado</returns>
    Task<bool> RemoverAsync(IdDonoDocumento id, CancellationToken cancellationToken = default);
}
