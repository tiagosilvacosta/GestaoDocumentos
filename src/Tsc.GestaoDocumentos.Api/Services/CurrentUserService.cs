using System.Security.Claims;
using Tsc.GestaoDocumentos.Domain.Organizacoes;
using Tsc.GestaoDocumentos.Domain.Usuarios;

namespace Tsc.GestaoDocumentos.Api.Services;

/// <summary>
/// Implementação do serviço que fornece informações do usuário atual logado
/// através do contexto HTTP e claims do JWT token.
/// </summary>
public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IdUsuario IdUsuario
    {
        get
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? _httpContextAccessor.HttpContext?.User?.FindFirst("sub")?.Value
                           ?? _httpContextAccessor.HttpContext?.User?.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                throw new UnauthorizedAccessException("Usuário não está autenticado ou ID do usuário é inválido");
            }

            return new IdUsuario(userId);
        }
    }

    public string UserName
    {
        get
        {
            var userName = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value
                        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("username")?.Value
                        ?? _httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value;

            if (string.IsNullOrEmpty(userName))
            {
                throw new UnauthorizedAccessException("Nome do usuário não encontrado no token");
            }

            return userName;
        }
    }

    public string Email
    {
        get
        {
            var email = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email)?.Value
                     ?? _httpContextAccessor.HttpContext?.User?.FindFirst("email")?.Value;

            if (string.IsNullOrEmpty(email))
            {
                throw new UnauthorizedAccessException("Email do usuário não encontrado no token");
            }

            return email;
        }
    }

    public IdOrganizacao IdOrganizacao
    {
        get
        {
            var organizacaoIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("organizationId")?.Value
                                  ?? _httpContextAccessor.HttpContext?.User?.FindFirst("orgId")?.Value
                                  ?? _httpContextAccessor.HttpContext?.User?.FindFirst("tenantId")?.Value;

            if (string.IsNullOrEmpty(organizacaoIdClaim) || !Guid.TryParse(organizacaoIdClaim, out var organizacaoId))
            {
                throw new UnauthorizedAccessException("ID da organização não encontrado no token ou é inválido");
            }

            return IdOrganizacao.CriarDeGuid(organizacaoId);
        }
    }
}