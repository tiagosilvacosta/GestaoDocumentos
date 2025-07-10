using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tsc.GestaoDocumentos.Domain.Organizacoes;

namespace Tsc.GestaoDocumentos.Api.Controllers;

/// <summary>
/// Controller de exemplo para demonstrar o uso das interfaces de contexto
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ExemploController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IContextoOrganizacao _contextoOrganizacao;
    private readonly ILogger<ExemploController> _logger;

    public ExemploController(
        ICurrentUserService currentUserService,
        IContextoOrganizacao contextoOrganizacao,
        ILogger<ExemploController> logger)
    {
        _currentUserService = currentUserService;
        _contextoOrganizacao = contextoOrganizacao;
        _logger = logger;
    }

    /// <summary>
    /// Retorna informações do usuário atual logado
    /// </summary>
    /// <returns>Dados do usuário atual</returns>
    [HttpGet("usuario-atual")]
    public ActionResult<Models.ResponseBaseModel<object>> ObterUsuarioAtual()
    {
        try
        {
            var dadosUsuario = new
            {
                IdUsuario = _currentUserService.IdUsuario.Valor,
                UserName = _currentUserService.UserName,
                Email = _currentUserService.Email,
                IdOrganizacao = _currentUserService.IdOrganizacao.Valor,
                TenantSlug = _contextoOrganizacao.TenantSlug
            };

            return Ok(Models.ResponseBaseModel<object>.CreateSuccess(dadosUsuario, "Dados do usuário obtidos com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acesso não autorizado ao obter dados do usuário");
            return Unauthorized(Models.ResponseBaseModel<object>.CreateError("Usuário não autorizado"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados do usuário atual");
            return StatusCode(500, Models.ResponseBaseModel<object>.CreateError("Erro interno do servidor"));
        }
    }

    /// <summary>
    /// Retorna informações da organização atual
    /// </summary>
    /// <returns>Dados da organização atual</returns>
    [HttpGet("organizacao-atual")]
    public ActionResult<Models.ResponseBaseModel<object>> ObterOrganizacaoAtual()
    {
        try
        {
            var dadosOrganizacao = new
            {
                IdOrganizacao = _contextoOrganizacao.IdOrganizacao.Valor,
                TenantSlug = _contextoOrganizacao.TenantSlug
            };

            return Ok(Models.ResponseBaseModel<object>.CreateSuccess(dadosOrganizacao, "Dados da organização obtidos com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acesso não autorizado ao obter dados da organização");
            return Unauthorized(Models.ResponseBaseModel<object>.CreateError("Usuário não autorizado"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados da organização atual");
            return StatusCode(500, Models.ResponseBaseModel<object>.CreateError("Erro interno do servidor"));
        }
    }
}