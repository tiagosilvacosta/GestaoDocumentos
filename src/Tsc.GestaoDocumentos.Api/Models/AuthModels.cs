using System.ComponentModel.DataAnnotations;

namespace Tsc.GestaoDocumentos.Api.Models;

/// <summary>
/// Modelo base para todas as respostas da API
/// </summary>
/// <typeparam name="T">Tipo dos dados retornados</typeparam>
public class ResponseBaseModel<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public IEnumerable<string> Errors { get; set; } = new List<string>();

    public static ResponseBaseModel<T> CreateSuccess(T data, string message = "Opera��o realizada com sucesso")
    {
        return new ResponseBaseModel<T>
        {
            Success = true,
            Message = message,
            Data = data,
            Errors = new List<string>()
        };
    }

    public static ResponseBaseModel<T> CreateError(string message, IEnumerable<string>? errors = null)
    {
        return new ResponseBaseModel<T>
        {
            Success = false,
            Message = message,
            Data = default,
            Errors = errors ?? new List<string> { message }
        };
    }
}

/// <summary>
/// Modelo para requisi��o de login
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "Email � obrigat�rio")]
    [EmailAddress(ErrorMessage = "Formato de email inv�lido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha � obrigat�ria")]
    public string Password { get; set; } = string.Empty;

    public string? TenantSlug { get; set; }
}

/// <summary>
/// Modelo para resposta de login
/// </summary>
public class LoginResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
    public UsuarioInfo Usuario { get; set; } = new();
}

/// <summary>
/// Modelo para resposta de refresh token
/// </summary>
public class RefreshTokenResponse
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiresAt { get; set; }
}

/// <summary>
/// Informa��es b�sicas do usu�rio logado
/// </summary>
public class UsuarioInfo
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid OrganizacaoId { get; set; }
    public string TenantSlug { get; set; } = string.Empty;
}