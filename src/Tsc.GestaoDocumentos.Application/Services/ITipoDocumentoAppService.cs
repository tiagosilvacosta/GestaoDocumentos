using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Interface do serviço de aplicação para gerenciamento de Tipos de Documento.
/// Responsável por orquestrar operações relacionadas a Tipos de Documento.
/// </summary>
public interface ITipoDocumentoAppService
{
    /// <summary>
    /// Obtém um tipo de documento por ID.
    /// </summary>
    /// <param name="id">ID do tipo de documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de documento ou null se não encontrado</returns>
    Task<TipoDocumentoDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os tipos de documento do tenant atual com paginação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de tipos de documento</returns>
    Task<PagedResult<TipoDocumentoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo tipo de documento.
    /// </summary>
    /// <param name="createTipoDocumento">Dados para criação do tipo de documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de documento criado</returns>
    Task<TipoDocumentoDto> CriarAsync(CreateTipoDocumentoDto createTipoDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um tipo de documento existente.
    /// </summary>
    /// <param name="id">ID do tipo de documento a ser atualizado</param>
    /// <param name="updateTipoDocumento">Dados para atualização do tipo de documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de documento atualizado</returns>
    Task<TipoDocumentoDto> AtualizarAsync(Guid id, UpdateTipoDocumentoDto updateTipoDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vincula tipos de dono a um tipo de documento.
    /// </summary>
    /// <param name="id">ID do tipo de documento</param>
    /// <param name="vincularTipoDono">IDs dos tipos de dono a serem vinculados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de documento atualizado</returns>
    Task<TipoDocumentoDto> VincularTiposDonoAsync(Guid id, VincularTipoDonoDto vincularTipoDono, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um tipo de documento.
    /// </summary>
    /// <param name="id">ID do tipo de documento a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, false se não encontrado</returns>
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
