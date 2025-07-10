using Tsc.GestaoDocumentos.Application.DTOs;
using Tsc.GestaoDocumentos.Application.DTOs.Common;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Application.Services;

/// <summary>
/// Interface do serviço de aplicação para gerenciamento de Usuários.
/// Responsável por orquestrar operações relacionadas a Usuários.
/// </summary>
public interface IUsuarioAppService
{
    /// <summary>
    /// Obtém um usuário por ID.
    /// </summary>
    /// <param name="id">ID do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário ou null se não encontrado</returns>
    Task<UsuarioDto?> ObterPorIdAsync(IdUsuario id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Obtém todos os usuários do tenant atual com paginação.
    /// </summary>
    /// <param name="request">Parâmetros de paginação</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Lista paginada de usuários</returns>
    Task<PagedResult<UsuarioDto>> ObterTodosAsync(PagedRequest request, CancellationToken cancellationToken = default);

    /// <summary>
    /// Cria um novo usuário.
    /// </summary>
    /// <param name="createUsuario">Dados para criação do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário criado</returns>
    Task<UsuarioDto> CriarAsync(CreateUsuarioDto createUsuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Atualiza um usuário existente.
    /// </summary>
    /// <param name="id">ID do usuário a ser atualizado</param>
    /// <param name="updateUsuario">Dados para atualização do usuário</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>Dados do usuário atualizado</returns>
    Task<UsuarioDto> AtualizarAsync(Guid id, UpdateUsuarioDto updateUsuario, CancellationToken cancellationToken = default);

    /// <summary>
    /// Remove um usuário.
    /// </summary>
    /// <param name="id">ID do usuário a ser removido</param>
    /// <param name="cancellationToken">Token de cancelamento</param>
    /// <returns>True se removido com sucesso, false se não encontrado</returns>
    Task<bool> RemoverAsync(Guid id, CancellationToken cancellationToken = default);
}
