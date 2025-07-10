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

    public static ResponseBaseModel<T> CreateSuccess(T data, string message = "Operação realizada com sucesso")
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
/// Modelo para requisição de login
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "Email é obrigatório")]
    [EmailAddress(ErrorMessage = "Formato de email inválido")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Senha é obrigatória")]
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
/// Informações básicas do usuário logado
/// </summary>
public class UsuarioInfo
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public Guid OrganizacaoId { get; set; }
    public string TenantSlug { get; set; } = string.Empty;
}