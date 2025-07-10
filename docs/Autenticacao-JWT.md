# Implementa��o de Autentica��o JWT

## Vis�o Geral

Este documento descreve a implementa��o das interfaces `IContextoOrganizacao` e `ICurrentUserService` para autentica��o JWT no projeto Gest�o de Documentos.

## Interfaces Implementadas

### 1. ICurrentUserService
**Localiza��o:** `src\Tsc.GestaoDocumentos.Api\Services\CurrentUserService.cs`

Fornece informa��es do usu�rio atual logado atrav�s do JWT token:
- `IdUsuario`: ID �nico do usu�rio
- `UserName`: Nome do usu�rio  
- `Email`: Email do usu�rio
- `IdOrganizacao`: ID da organiza��o do usu�rio

### 2. IContextoOrganizacao
**Localiza��o:** `src\Tsc.GestaoDocumentos.Api\Services\ContextoOrganizacao.cs`

Fornece informa��es da organiza��o atual:
- `IdOrganizacao`: ID da organiza��o
- `TenantSlug`: Identificador �nico da organiza��o (slug)

## Claims JWT Esperadas

As implementa��es buscam as seguintes claims no token JWT:

### Usu�rio
- `ClaimTypes.NameIdentifier` ou `sub` ou `userId`: ID do usu�rio
- `ClaimTypes.Name` ou `username` ou `preferred_username`: Nome do usu�rio
- `ClaimTypes.Email` ou `email`: Email do usu�rio

### Organiza��o
- `organizationId` ou `orgId` ou `tenantId`: ID da organiza��o
- `tenantSlug` ou `organizationSlug` ou `orgSlug`: Slug da organiza��o

## Configura��o

### 1. Configura��o no appsettings.json
```json
{
  "Jwt": {
    "Issuer": "GestaoDocumentos.Api",
    "Audience": "GestaoDocumentos.Client", 
    "SecretKey": "sua-chave-secreta-muito-segura-com-pelo-menos-256-bits-aqui"
  }
}
```

### 2. Registro de Depend�ncias
As implementa��es s�o registradas automaticamente no `DependencyInjection.cs` da API:

```csharp
services.AddScoped<ICurrentUserService, CurrentUserService>();
services.AddScoped<IContextoOrganizacao, ContextoOrganizacao>();
```

### 3. Configura��o JWT
A autentica��o JWT � configurada com:
- Valida��o de issuer, audience, lifetime e signing key
- Tratamento personalizado de erros de autentica��o e autoriza��o
- Headers de Authorization com esquema Bearer

## Uso

### Em Controllers
```csharp
[ApiController]
[Authorize]
public class MeuController : ControllerBase
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IContextoOrganizacao _contextoOrganizacao;

    public MeuController(ICurrentUserService currentUserService, IContextoOrganizacao contextoOrganizacao)
    {
        _currentUserService = currentUserService;
        _contextoOrganizacao = contextoOrganizacao;
    }

    [HttpGet]
    public ActionResult MinhaAction()
    {
        var userId = _currentUserService.IdUsuario;
        var orgId = _contextoOrganizacao.IdOrganizacao;
        // ...
    }
}
```

### Em Servi�os de Aplica��o
```csharp
public class MeuServicoApp
{
    private readonly ICurrentUserService _currentUserService;

    public MeuServicoApp(ICurrentUserService currentUserService)
    {
        _currentUserService = currentUserService;
    }

    public async Task ProcessarAsync()
    {
        var usuarioId = _currentUserService.IdUsuario;
        var organizacaoId = _currentUserService.IdOrganizacao;
        // ...
    }
}
```

## Autentica��o

### Login
O endpoint `/api/auth/login` aceita email e senha e retorna um token JWT v�lido por 8 horas.

**Exemplo de request:**
```json
{
  "email": "admin@exemplo.com",
  "password": "123456"
}
```

**Exemplo de response:**
```json
{
  "success": true,
  "message": "Login realizado com sucesso",
  "data": {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "expiresAt": "2024-01-01T08:00:00Z",
    "usuario": {
      "id": "guid",
      "nome": "Administrador", 
      "email": "admin@exemplo.com",
      "organizacaoId": "guid",
      "tenantSlug": "exemplo"
    }
  }
}
```

### Refresh Token
O endpoint `/api/auth/refresh` permite renovar um token v�lido.

### Uso do Token
Incluir o token no header Authorization:
```
Authorization: Bearer {token}
```

## Tratamento de Erros

As implementa��es lan�am `UnauthorizedAccessException` quando:
- Token JWT � inv�lido ou expirado
- Claims obrigat�rias est�o ausentes
- Valores das claims s�o inv�lidos

Estes erros s�o automaticamente convertidos em respostas HTTP 401 pela configura��o JWT.

## Swagger/OpenAPI

A API est� configurada com suporte ao Swagger incluindo autentica��o Bearer JWT.

## Considera��es de Seguran�a

1. **Chave Secreta**: Use uma chave forte de pelo menos 256 bits
2. **HTTPS**: Sempre usar HTTPS em produ��o
3. **Tempo de Vida**: Tokens t�m validade de 8 horas
4. **Claims**: Valida��o rigorosa de todas as claims obrigat�rias

## Pr�ximos Passos

1. Implementar valida��o real de usu�rio/senha contra banco de dados
2. Adicionar suporte a refresh tokens persistentes
3. Implementar logout com blacklist de tokens
4. Adicionar logs de auditoria para autentica��o