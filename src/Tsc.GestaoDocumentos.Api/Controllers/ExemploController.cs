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
    /// Retorna informa��es do usu�rio atual logado
    /// </summary>
    /// <returns>Dados do usu�rio atual</returns>
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

            return Ok(Models.ResponseBaseModel<object>.CreateSuccess(dadosUsuario, "Dados do usu�rio obtidos com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acesso n�o autorizado ao obter dados do usu�rio");
            return Unauthorized(Models.ResponseBaseModel<object>.CreateError("Usu�rio n�o autorizado"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados do usu�rio atual");
            return StatusCode(500, Models.ResponseBaseModel<object>.CreateError("Erro interno do servidor"));
        }
    }

    /// <summary>
    /// Retorna informa��es da organiza��o atual
    /// </summary>
    /// <returns>Dados da organiza��o atual</returns>
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

            return Ok(Models.ResponseBaseModel<object>.CreateSuccess(dadosOrganizacao, "Dados da organiza��o obtidos com sucesso"));
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Acesso n�o autorizado ao obter dados da organiza��o");
            return Unauthorized(Models.ResponseBaseModel<object>.CreateError("Usu�rio n�o autorizado"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao obter dados da organiza��o atual");
            return StatusCode(500, Models.ResponseBaseModel<object>.CreateError("Erro interno do servidor"));
        }
    }
}