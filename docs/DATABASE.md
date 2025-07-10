# Estrutura do Banco de Dados - Sistema de Gestão de Documentos

## Visão Geral

O sistema utiliza **Entity Framework Core 9.0.7** para gerenciar a estrutura do banco de dados SQL Server. A arquitetura implementa o padrão **Multi-tenancy** usando o conceito de organizações.

## Configuração de Conexão

A string de conexão está configurada no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestaoDocumentosDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Estrutura das Tabelas

### 1. Tenants (Organizações)
Tabela principal para multi-tenancy.

**Campos:**
- `Id` (uniqueidentifier, PK) - Identificador único da organização
- `NomeOrganizacao` (nvarchar(255)) - Nome da organização
- `Slug` (nvarchar(50), único) - Identificador amigável
- `Status` (int) - Status da organização
- `DataCriacao` (datetime2) - Data de criação
- `DataAtualizacao` (datetime2) - Data da última atualização
- `UsuarioCriacao` (uniqueidentifier) - ID do usuário que criou
- `UsuarioUltimaAlteracao` (uniqueidentifier) - ID do último usuário que alterou

**Índices:**
- `IX_Tenants_Slug` (único)
- `IX_Tenants_NomeOrganizacao`
- `IX_Tenants_Status`

### 2. Usuarios
Gerenciamento de usuários do sistema.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `Nome` (nvarchar(255))
- `Email` (nvarchar(255))
- `Login` (nvarchar(100))
- `SenhaHash` (nvarchar(500))
- `Status` (int)
- `Perfil` (int)

**Índices:**
- `IX_Usuarios_IdOrganizacao_Email` (único)
- `IX_Usuarios_IdOrganizacao_Login` (único)
- `IX_Usuarios_IdOrganizacao_Status`
- `IX_Usuarios_IdOrganizacao_Perfil`

### 3. TiposDono
Categorização dos donos de documentos (ex: Pessoa Física, Empresa, etc.).

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `Nome` (nvarchar(255))

**Índices:**
- `IX_TiposDono_IdOrganizacao_Nome` (único)

### 4. TiposDocumento
Categorização dos tipos de documento (ex: RG, CPF, Contrato, etc.).

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `Nome` (nvarchar(255))
- `PermiteMultiplosDocumentos` (bit)

**Índices:**
- `IX_TiposDocumento_IdOrganizacao_Nome` (único)

### 5. TipoDonoTipoDocumento
Tabela de relacionamento many-to-many entre TiposDono e TiposDocumento.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier)
- `IdTipoDono` (uniqueidentifier, FK para TiposDono)
- `IdTipoDocumento` (uniqueidentifier, FK para TiposDocumento)

**Índices:**
- `IX_TipoDonoTipoDocumento_IdOrganizacao_IdTipoDono_IdTipoDocumento` (único)

### 6. DonosDocumento
Registro dos donos/proprietários de documentos.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `NomeAmigavel` (nvarchar(255))
- `IdTipoDono` (uniqueidentifier, FK para TiposDono)

**Índices:**
- `IX_DonosDocumento_IdOrganizacao_NomeAmigavel`
- `IX_DonosDocumento_IdOrganizacao_IdTipoDono`

### 7. Documentos
Tabela principal de documentos.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `NomeArquivo` (nvarchar(255))
- `ChaveArmazenamento` (nvarchar(500))
- `DataUpload` (datetime2)
- `TamanhoArquivo` (bigint)
- `TipoArquivo` (nvarchar(50))
- `Versao` (int)
- `Status` (int)
- `IdTipoDocumento` (uniqueidentifier, FK para TiposDocumento)

**Índices:**
- `IX_Documentos_IdOrganizacao_ChaveArmazenamento` (único)
- `IX_Documentos_IdOrganizacao_IdTipoDocumento`
- `IX_Documentos_IdOrganizacao_Status`
- `IX_Documentos_IdOrganizacao_DataUpload`

### 8. DocumentoDonoDocumento
Tabela de relacionamento many-to-many entre Documentos e DonosDocumento.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier)
- `IdDocumento` (uniqueidentifier, FK para Documentos)
- `IdDonoDocumento` (uniqueidentifier, FK para DonosDocumento)

**Índices:**
- `IX_DocumentoDonoDocumento_IdOrganizacao_IdDocumento_IdDonoDocumento` (único)

### 9. LogsAuditoria
Sistema de auditoria para rastreamento de operações.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `IdUsuario` (uniqueidentifier)
- `EntidadeAfetada` (nvarchar(100))
- `IdEntidade` (uniqueidentifier)
- `Operacao` (int)
- `DadosAnteriores` (nvarchar(max))
- `DadosNovos` (nvarchar(max))
- `DataHoraOperacao` (datetime2)
- `IpUsuario` (nvarchar(45))
- `UserAgent` (nvarchar(500))

**Índices:**
- `IX_LogsAuditoria_IdOrganizacao_IdUsuario`
- `IX_LogsAuditoria_IdOrganizacao_EntidadeAfetada_EntidadeId`
- `IX_LogsAuditoria_IdOrganizacao_Operacao`
- `IX_LogsAuditoria_IdOrganizacao_DataHoraOperacao`

## Relacionamentos e Restrições

### Cascade Delete
- **Organizações ? Usuários, TiposDono, TiposDocumento, DonosDocumento, LogsAuditoria**: CASCADE
- **Demais relacionamentos**: RESTRICT (para evitar exclusões acidentais)

### Multi-tenancy
- Todas as entidades (exceto Tenants) possuem `IdOrganizacao`
- Filtros globais aplicados automaticamente no DbContext
- Índices compostos incluem sempre `IdOrganizacao` como primeiro campo

## Comandos de Migração

### Criar Nova Migração
```bash
dotnet ef migrations add NomeDaMigracao --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

### Aplicar Migrações
```bash
dotnet ef database update --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

### Reverter Migração
```bash
dotnet ef database update NomeMigracaoAnterior --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

### Remover Última Migração
```bash
dotnet ef migrations remove --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

## Observações Importantes

1. **GUIDs como Chaves Primárias**: Todas as entidades usam GUIDs como identificadores
2. **Value Objects**: IDs são implementados como Value Objects para type-safety
3. **Auditoria Automática**: Entidades que herdam de `EntidadeComAuditoria` têm rastreamento automático
4. **Conversores Personalizados**: Value Objects têm conversores específicos para o EF Core
5. **Validation**: Todas as validações são implementadas no nível de domínio

## Estrutura de Arquivos

```
src/Tsc.GestaoDocumentos.Infrastructure/
??? Data/
?   ??? GestaoDocumentosDbContext.cs
?   ??? GestaoDocumentosDbContextFactory.cs
??? Migrations/
?   ??? [arquivos de migração]
??? Documentos/
?   ??? ConfiguracaoDocumento.cs
??? Usuarios/
?   ??? ConfiguracaoUsuario.cs
??? Organizacoes/
?   ??? ConfiguracaoOrganizacao.cs
??? Logs/
    ??? ConfiguracaoLogAuditoria.cs
```

## Status Atual

? **Estrutura do Banco Criada**  
? **Migrações Aplicadas**  
? **Relacionamentos Configurados**  
? **Índices Otimizados**  
? **Multi-tenancy Implementado**  
? **Sistema de Auditoria Configurado**