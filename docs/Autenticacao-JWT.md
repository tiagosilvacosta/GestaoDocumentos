# Implementação de Autenticação JWT

## Visão Geral

Este documento descreve a implementação das interfaces `IContextoOrganizacao` e `ICurrentUserService` para autenticação JWT no projeto Gestão de Documentos.

## Interfaces Implementadas

### 1. ICurrentUserService
**Localização:** `src\Tsc.GestaoDocumentos.Api\Services\CurrentUserService.cs`

Fornece informações do usuário atual logado através do JWT token:
- `IdUsuario`: ID único do usuário
- `UserName`: Nome do usuário  
- `Email`: Email do usuário
- `IdOrganizacao`: ID da organização do usuário

### 2. IContextoOrganizacao
**Localização:** `src\Tsc.GestaoDocumentos.Api\Services\ContextoOrganizacao.cs`

Fornece informações da organização atual:
- `IdOrganizacao`: ID da organização
- `TenantSlug`: Identificador único da organização (slug)

## Claims JWT Esperadas

As implementações buscam as seguintes claims no token JWT:

### Usuário
- `ClaimTypes.NameIdentifier` ou `sub` ou `userId`: ID do usuário
- `ClaimTypes.Name` ou `username` ou `preferred_username`: Nome do usuário
- `ClaimTypes.Email` ou `email`: Email do usuário

### Organização
- `organizationId` ou `orgId` ou `tenantId`: ID da organização
- `tenantSlug` ou `organizationSlug` ou `orgSlug`: Slug da organização

## Configuração

### 1. Configuração no appsettings.json
```json
{
  "Jwt": {
    "Issuer": "GestaoDocumentos.Api",
    "Audience": "GestaoDocumentos.Client", 
    "SecretKey": "sua-chave-secreta-muito-segura-com-pelo-menos-256-bits-aqui"
  }
}
```

### 2. Registro de Dependências
As implementações são registradas automaticamente no `DependencyInjection.cs` da API:

```csharp
services.AddScoped<ICurrentUserService, CurrentUserService>();
services.AddScoped<IContextoOrganizacao, ContextoOrganizacao>();
```

### 3. Configuração JWT
A autenticação JWT é configurada com:
- Validação de issuer, audience, lifetime e signing key
- Tratamento personalizado de erros de autenticação e autorização
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

### Em Serviços de Aplicação
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

## Autenticação

### Login
O endpoint `/api/auth/login` aceita email e senha e retorna um token JWT válido por 8 horas.

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
O endpoint `/api/auth/refresh` permite renovar um token válido.

### Uso do Token
Incluir o token no header Authorization:
```
Authorization: Bearer {token}
```

## Tratamento de Erros

As implementações lançam `UnauthorizedAccessException` quando:
- Token JWT é inválido ou expirado
- Claims obrigatórias estão ausentes
- Valores das claims são inválidos

Estes erros são automaticamente convertidos em respostas HTTP 401 pela configuração JWT.

## Swagger/OpenAPI

A API está configurada com suporte ao Swagger incluindo autenticação Bearer JWT.

## Considerações de Segurança

1. **Chave Secreta**: Use uma chave forte de pelo menos 256 bits
2. **HTTPS**: Sempre usar HTTPS em produção
3. **Tempo de Vida**: Tokens têm validade de 8 horas
4. **Claims**: Validação rigorosa de todas as claims obrigatórias

## Próximos Passos

1. Implementar validação real de usuário/senha contra banco de dados
2. Adicionar suporte a refresh tokens persistentes
3. Implementar logout com blacklist de tokens
4. Adicionar logs de auditoria para autenticação