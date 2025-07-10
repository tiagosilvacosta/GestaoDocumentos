using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Interface do serviço de aplicação para gerenciamento de Documentos.
/// Responsável por orquestrar operações relacionadas a Documentos.
/// </summary>
public interface IDocumentoAppService
{
    /// <summary>
    /// Obtém um documento por ID.
    /// </summary>
    /// <param name="id">ID do documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do documento ou null se não encontrado</returns>
    Task<DocumentoDto?> ObterPorIdAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os documentos do tenant atual com paginação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de documentos</returns>
    Task<PagedResult<DocumentoDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo documento.
    /// </summary>
    /// <param name="createDocumento">Dados para criação do documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do documento criado</returns>
    Task<DocumentoDto> CriarAsync(CreateDocumentoDto createDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um documento existente.
    /// </summary>
    /// <param name="id">ID do documento a ser atualizado</param>
    /// <param name="updateDocumento">Dados para atualização do documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do documento atualizado</returns>
    Task<DocumentoDto> AtualizarAsync(Guid id, UpdateDocumentoDto updateDocumento, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria uma nova versão de um documento existente.
    /// </summary>
    /// <param name="id">ID do documento</param>
    /// <param name="novaVersao">Dados da nova versão</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do documento com nova versão</returns>
    Task<DocumentoDto> CriarNovaVersaoAsync(Guid id, NovaVersaoDocumentoDto novaVersao, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um documento.
    /// </summary>
    /// <param name="id">ID do documento a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, false se não encontrado</returns>
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Faz download de um documento.
    /// </summary>
    /// <param name="id">ID do documento</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Stream do arquivo e informações do documento</returns>
    Task<(Stream arquivo, string nomeArquivo, string tipoArquivo)?> FazerDownloadAsync(Guid id, CancellationToken cancellationToken = default);
}
