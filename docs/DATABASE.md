# Estrutura do Banco de Dados - Sistema de Gest�o de Documentos

## Vis�o Geral

O sistema utiliza **Entity Framework Core 9.0.7** para gerenciar a estrutura do banco de dados SQL Server. A arquitetura implementa o padr�o **Multi-tenancy** usando o conceito de organiza��es.

## Configura��o de Conex�o

A string de conex�o est� configurada no arquivo `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestaoDocumentosDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

## Estrutura das Tabelas

### 1. Tenants (Organiza��es)
Tabela principal para multi-tenancy.

**Campos:**
- `Id` (uniqueidentifier, PK) - Identificador �nico da organiza��o
- `NomeOrganizacao` (nvarchar(255)) - Nome da organiza��o
- `Slug` (nvarchar(50), �nico) - Identificador amig�vel
- `Status` (int) - Status da organiza��o
- `DataCriacao` (datetime2) - Data de cria��o
- `DataAtualizacao` (datetime2) - Data da �ltima atualiza��o
- `UsuarioCriacao` (uniqueidentifier) - ID do usu�rio que criou
- `UsuarioUltimaAlteracao` (uniqueidentifier) - ID do �ltimo usu�rio que alterou

**�ndices:**
- `IX_Tenants_Slug` (�nico)
- `IX_Tenants_NomeOrganizacao`
- `IX_Tenants_Status`

### 2. Usuarios
Gerenciamento de usu�rios do sistema.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `Nome` (nvarchar(255))
- `Email` (nvarchar(255))
- `Login` (nvarchar(100))
- `SenhaHash` (nvarchar(500))
- `Status` (int)
- `Perfil` (int)

**�ndices:**
- `IX_Usuarios_IdOrganizacao_Email` (�nico)
- `IX_Usuarios_IdOrganizacao_Login` (�nico)
- `IX_Usuarios_IdOrganizacao_Status`
- `IX_Usuarios_IdOrganizacao_Perfil`

### 3. TiposDono
Categoriza��o dos donos de documentos (ex: Pessoa F�sica, Empresa, etc.).

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `Nome` (nvarchar(255))

**�ndices:**
- `IX_TiposDono_IdOrganizacao_Nome` (�nico)

### 4. TiposDocumento
Categoriza��o dos tipos de documento (ex: RG, CPF, Contrato, etc.).

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `Nome` (nvarchar(255))
- `PermiteMultiplosDocumentos` (bit)

**�ndices:**
- `IX_TiposDocumento_IdOrganizacao_Nome` (�nico)

### 5. TipoDonoTipoDocumento
Tabela de relacionamento many-to-many entre TiposDono e TiposDocumento.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier)
- `IdTipoDono` (uniqueidentifier, FK para TiposDono)
- `IdTipoDocumento` (uniqueidentifier, FK para TiposDocumento)

**�ndices:**
- `IX_TipoDonoTipoDocumento_IdOrganizacao_IdTipoDono_IdTipoDocumento` (�nico)

### 6. DonosDocumento
Registro dos donos/propriet�rios de documentos.

**Campos:**
- `Id` (uniqueidentifier, PK)
- `IdOrganizacao` (uniqueidentifier, FK para Tenants)
- `NomeAmigavel` (nvarchar(255))
- `IdTipoDono` (uniqueidentifier, FK para TiposDono)

**�ndices:**
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

**�ndices:**
- `IX_Documentos_IdOrganizacao_ChaveArmazenamento` (�nico)
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

**�ndices:**
- `IX_DocumentoDonoDocumento_IdOrganizacao_IdDocumento_IdDonoDocumento` (�nico)

### 9. LogsAuditoria
Sistema de auditoria para rastreamento de opera��es.

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

**�ndices:**
- `IX_LogsAuditoria_IdOrganizacao_IdUsuario`
- `IX_LogsAuditoria_IdOrganizacao_EntidadeAfetada_EntidadeId`
- `IX_LogsAuditoria_IdOrganizacao_Operacao`
- `IX_LogsAuditoria_IdOrganizacao_DataHoraOperacao`

## Relacionamentos e Restri��es

### Cascade Delete
- **Organiza��es ? Usu�rios, TiposDono, TiposDocumento, DonosDocumento, LogsAuditoria**: CASCADE
- **Demais relacionamentos**: RESTRICT (para evitar exclus�es acidentais)

### Multi-tenancy
- Todas as entidades (exceto Tenants) possuem `IdOrganizacao`
- Filtros globais aplicados automaticamente no DbContext
- �ndices compostos incluem sempre `IdOrganizacao` como primeiro campo

## Comandos de Migra��o

### Criar Nova Migra��o
```bash
dotnet ef migrations add NomeDaMigracao --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

### Aplicar Migra��es
```bash
dotnet ef database update --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

### Reverter Migra��o
```bash
dotnet ef database update NomeMigracaoAnterior --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

### Remover �ltima Migra��o
```bash
dotnet ef migrations remove --project src\Tsc.GestaoDocumentos.Infrastructure --startup-project src\Tsc.GestaoDocumentos.Api
```

## Observa��es Importantes

1. **GUIDs como Chaves Prim�rias**: Todas as entidades usam GUIDs como identificadores
2. **Value Objects**: IDs s�o implementados como Value Objects para type-safety
3. **Auditoria Autom�tica**: Entidades que herdam de `EntidadeComAuditoria` t�m rastreamento autom�tico
4. **Conversores Personalizados**: Value Objects t�m conversores espec�ficos para o EF Core
5. **Validation**: Todas as valida��es s�o implementadas no n�vel de dom�nio

## Estrutura de Arquivos

```
src/Tsc.GestaoDocumentos.Infrastructure/
??? Data/
?   ??? GestaoDocumentosDbContext.cs
?   ??? GestaoDocumentosDbContextFactory.cs
??? Migrations/
?   ??? [arquivos de migra��o]
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
? **Migra��es Aplicadas**  
? **Relacionamentos Configurados**  
? **�ndices Otimizados**  
? **Multi-tenancy Implementado**  
? **Sistema de Auditoria Configurado**