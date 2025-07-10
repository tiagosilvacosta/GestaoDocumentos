using System.Security.Claims;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Api.Services;

/// <summary>
/// Implementação do contexto de organização que obtém informações da organização atual
/// através do contexto HTTP e claims do JWT token.
/// </summary>
public class ContextoOrganizacao : IContextoOrganizacao
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ContextoOrganizacao(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
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

    public string TenantSlug
    {
        get
        {
            var tenantSlug = _httpContextAccessor.HttpContext?.User?.FindFirst("tenantSlug")?.Value
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirst("organizationSlug")?.Value
                          ?? _httpContextAccessor.HttpContext?.User?.FindFirst("orgSlug")?.Value;

            if (string.IsNullOrEmpty(tenantSlug))
            {
                throw new UnauthorizedAccessException("Slug da organização não encontrado no token");
            }

            return tenantSlug;
        }
    }
}