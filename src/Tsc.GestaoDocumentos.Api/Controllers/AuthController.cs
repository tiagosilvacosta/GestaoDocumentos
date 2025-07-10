using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Tsc.GestaoDocumentos.Api.Controllers;

/// <summary>
/// Controller responsável pela autenticação de usuários e geração de tokens JWT
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IConfiguration configuration, ILogger<AuthController> logger)
    {
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>
    /// Realiza o login do usuário e retorna um token JWT
    /// </summary>
    /// <param name="loginRequest">Dados de login</param>
    /// <returns>Token JWT e informações do usuário</returns>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<Models.ResponseBaseModel<Models.LoginResponse>>> Login([FromBody] Models.LoginRequest loginRequest)
    {
        try
        {
            // TODO: Implementar validação real de usuário e senha
            // Por enquanto, exemplo simplificado para demonstração
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest(Models.ResponseBaseModel<Models.LoginResponse>.CreateError("Email e senha são obrigatórios"));
            }

            // TODO: Validar credenciais contra o banco de dados
            // Este é apenas um exemplo - implementar validação real
            if (loginRequest.Email == "admin@exemplo.com" && loginRequest.Password == "123456")
            {
                var token = GerarTokenJwt(
                    userId: Guid.NewGuid(),
                    userName: "Administrador",
                    email: loginRequest.Email,
                    organizationId: Guid.NewGuid(),
                    tenantSlug: "exemplo"
                );

                var response = new Models.LoginResponse
                {
                    Token = token,
                    ExpiresAt = DateTime.UtcNow.AddHours(8),
                    Usuario = new Models.UsuarioInfo
                    {
                        Id = Guid.NewGuid(),
                        Nome = "Administrador",
                        Email = loginRequest.Email,
                        OrganizacaoId = Guid.NewGuid(),
                        TenantSlug = "exemplo"
                    }
                };

                return Ok(Models.ResponseBaseModel<Models.LoginResponse>.CreateSuccess(response, "Login realizado com sucesso"));
            }

            return Unauthorized(Models.ResponseBaseModel<Models.LoginResponse>.CreateError("Credenciais inválidas"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante o processo de login");
            return StatusCode(500, Models.ResponseBaseModel<Models.LoginResponse>.CreateError("Erro interno do servidor"));
        }
    }

    /// <summary>
    /// Atualiza o token JWT (refresh token)
    /// </summary>
    /// <returns>Novo token JWT</returns>
    [HttpPost("refresh")]
    [Authorize]
    public ActionResult<Models.ResponseBaseModel<Models.RefreshTokenResponse>> RefreshToken()
    {
        try
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var currentUserName = User.FindFirst(ClaimTypes.Name)?.Value;
            var currentUserEmail = User.FindFirst(ClaimTypes.Email)?.Value;
            var currentOrgId = User.FindFirst("organizationId")?.Value;
            var currentTenantSlug = User.FindFirst("tenantSlug")?.Value;

            if (string.IsNullOrEmpty(currentUserId) || !Guid.TryParse(currentUserId, out var userId))
            {
                return Unauthorized(Models.ResponseBaseModel<Models.RefreshTokenResponse>.CreateError("Token inválido"));
            }

            var newToken = GerarTokenJwt(
                userId: userId,
                userName: currentUserName ?? "",
                email: currentUserEmail ?? "",
                organizationId: Guid.Parse(currentOrgId ?? Guid.NewGuid().ToString()),
                tenantSlug: currentTenantSlug ?? ""
            );

            var response = new Models.RefreshTokenResponse
            {
                Token = newToken,
                ExpiresAt = DateTime.UtcNow.AddHours(8)
            };

            return Ok(Models.ResponseBaseModel<Models.RefreshTokenResponse>.CreateSuccess(response, "Token atualizado com sucesso"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante a atualização do token");
            return StatusCode(500, Models.ResponseBaseModel<Models.RefreshTokenResponse>.CreateError("Erro interno do servidor"));
        }
    }

    private string GerarTokenJwt(Guid userId, string userName, string email, Guid organizationId, string tenantSlug)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
            new Claim(ClaimTypes.Name, userName),
            new Claim(ClaimTypes.Email, email),
            new Claim("userId", userId.ToString()),
            new Claim("username", userName),
            new Claim("email", email),
            new Claim("organizationId", organizationId.ToString()),
            new Claim("orgId", organizationId.ToString()),
            new Claim("tenantId", organizationId.ToString()),
            new Claim("tenantSlug", tenantSlug),
            new Claim("organizationSlug", tenantSlug),
            new Claim("orgSlug", tenantSlug),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(8),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}