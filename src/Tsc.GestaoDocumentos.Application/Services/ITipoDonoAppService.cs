using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Documentos;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Interface do serviço de aplicação para gerenciamento de Tipos de Dono.
/// Responsável por orquestrar operações relacionadas a Tipos de Dono.
/// </summary>
public interface ITipoDonoAppService
{
    /// <summary>
    /// Obtém um tipo de dono por ID.
    /// </summary>
    /// <param name="id">ID do tipo de dono</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de dono ou null se não encontrado</returns>
    Task<TipoDonoDto?> ObterPorIdAsync(IdTipoDono id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os tipos de dono do tenant atual com paginação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de tipos de dono</returns>
    Task<PagedResult<TipoDonoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo tipo de dono.
    /// </summary>
    /// <param name="createTipoDono">Dados para criação do tipo de dono</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de dono criado</returns>
    Task<TipoDonoDto> CriarAsync(CreateTipoDonoDto createTipoDono, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um tipo de dono existente.
    /// </summary>
    /// <param name="id">ID do tipo de dono a ser atualizado</param>
    /// <param name="updateTipoDono">Dados para atualização do tipo de dono</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de dono atualizado</returns>
    Task<TipoDonoDto> AtualizarAsync(IdTipoDono id, UpdateTipoDonoDto updateTipoDono, CancellationToken cancellationToken = default);

    /// <summary>
    /// Vincula tipos de documento a um tipo de dono.
    /// </summary>
    /// <param name="id">ID do tipo de dono</param>
    /// <param name="vincularTipoDocumento">IDs dos tipos de documento a serem vinculados</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do tipo de dono atualizado</returns>
    Task<TipoDonoDto> VincularTiposDocumentoAsync(IdTipoDono id, VincularTipoDocumentoDto vincularTipoDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um tipo de dono.
    /// </summary>
    /// <param name="id">ID do tipo de dono a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, false se não encontrado</returns>
    Task<bool> RemoverAsync(IdTipoDono id, CancellationToken cancellationToken = default);
}
