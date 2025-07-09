# Documento de Requisitos: Sistema de Gerenciamento de Documentos Multi-Tenant: Nome do Projeto: Tsc.GestaoDocumentos

## 1. Visão Geral
Sistema multi-tenant para gerenciar documentos organizados por proprietários, com controle de tipos, versões e permissões de acesso. Cada tenant terá isolamento completo de dados e configurações.

## 2. Entidades Principais

### 2.1 Tenant
**Descrição:** Representa uma organização/cliente que utiliza o sistema com isolamento completo de dados.

**Atributos:**
- ID único (gerado automaticamente)
- Nome da organização
- Código/identificador único (slug)
- Status (Ativo/Inativo/Suspenso)
- Data de criação
- Data de expiração (se aplicável)
- Data da última alteração

### 2.2 Usuário
**Descrição:** Usuários do sistema vinculados a um tenant específico.

**Atributos:**
- ID único (gerado automaticamente)
- Tenant ID (FK)
- Nome
- Email
- Login
- Senha (hash)
- Status (Ativo/Inativo)
- Perfil/Role
- Data de criação
- Data da última alteração
- Último acesso

**Relacionamentos:**
- Pertence a um **Tenant**

### 2.3 Tipo de Dono
**Descrição:** Cadastro que categoriza os diferentes tipos de proprietários de documentos por tenant.

**Atributos:**
- ID único (gerado automaticamente)
- **Tenant ID (FK)**
- Nome (ex: "Pessoa Física", "Pessoa Jurídica", "Título a Receber")
- Data de criação
- Usuário que criou (FK)
- Data da última alteração
- Usuário que alterou (FK)

**Relacionamentos:**
- Pertence a um **Tenant**

### 2.4 Tipo de Documento
**Descrição:** Cadastro que define os tipos de documentos disponíveis no sistema por tenant.

**Atributos:**
- ID único (gerado automaticamente)
- **Tenant ID (FK)**
- Nome (ex: "RG", "CPF", "Nota Fiscal")
- Permite múltiplos documentos por dono (Sim/Não)
- Data de criação
- Usuário que criou (FK)
- Data da última alteração
- Usuário que alterou (FK)

**Relacionamentos:**
- Pertence a um **Tenant**
- Pode estar vinculado a múltiplos **Tipos de Dono** do mesmo tenant (relacionamento N:N)

### 2.5 Dono do Documento
**Descrição:** Container que agrupa documentos de um proprietário específico dentro de um tenant.

**Atributos:**
- ID único (gerado automaticamente)
- **Tenant ID (FK)**
- Nome amigável
- Tipo de Dono (FK)
- Data de criação
- Usuário que criou (FK)
- Data da última alteração
- Usuário que alterou (FK)

**Relacionamentos:**
- Pertence a um **Tenant**
- Possui um **Tipo de Dono** do mesmo tenant
- Pode ter múltiplos **Documentos**

### 2.6 Documento
**Descrição:** Representa um arquivo armazenado no sistema com seus metadados, isolado por tenant.

**Atributos:**
- ID único (gerado automaticamente)
- **Tenant ID (FK)**
- Nome do arquivo
- Chave de armazenamento (localização física do arquivo incluindo tenant)
- Data de upload
- Tamanho do arquivo
- Tipo de arquivo/extensão
- Versão
- Status (Ativo/Inativo)
- Data de criação
- Usuário que criou (FK)
- Data da última alteração
- Usuário que alterou (FK)

**Relacionamentos:**
- Pertence a um **Tenant**
- Possui um **Tipo de Documento** do mesmo tenant
- Pode estar vinculado a múltiplos **Donos do Documento** do mesmo tenant (relacionamento N:N)

### 2.7 Log de Auditoria
**Descrição:** Registra todas as operações realizadas no sistema por tenant.

**Atributos:**
- ID único (gerado automaticamente)
- **Tenant ID (FK)**
- Usuário (FK)
- Entidade afetada
- ID da entidade
- Operação (CREATE, UPDATE, DELETE, DOWNLOAD, etc.)
- Dados anteriores (JSON)
- Dados novos (JSON)
- Data/hora da operação
- IP do usuário
- User Agent

## 3. Regras de Negócio

### 3.1 Regras de Isolamento Multi-Tenant
- **RN001:** Todas as consultas devem incluir filtro por Tenant ID
- **RN002:** Usuários só podem acessar dados do seu próprio tenant
- **RN003:** Relacionamentos entre entidades só podem ocorrer dentro do mesmo tenant
- **RN004:** Chaves de armazenamento de arquivos devem incluir identificação do tenant

### 3.2 Regras de Vinculação
- **RN005:** Um **Documento** deve ter exatamente um **Tipo de Documento** do mesmo tenant
- **RN006:** Um **Documento** pode estar vinculado a múltiplos **Donos do Documento** do mesmo tenant
- **RN007:** Os **Donos do Documento** vinculados a um mesmo documento podem ser de **Tipos de Dono** diferentes, mas do mesmo tenant
- **RN008:** Um **Tipo de Documento** pode estar vinculado a múltiplos **Tipos de Dono** do mesmo tenant

### 3.3 Regras de Validação
- **RN009:** Ao incluir um **Documento** em um **Dono do Documento**, o sistema deve verificar se o **Tipo de Documento** está vinculado ao **Tipo de Dono** do proprietário dentro do mesmo tenant
- **RN010:** Se o **Tipo de Documento** não permitir múltiplos documentos, não será possível criar um novo documento ativo do mesmo tipo para o mesmo **Dono do Documento**
- **RN011:** Se o **Tipo de Documento** permitir múltiplos documentos, será possível ter vários documentos ativos do mesmo tipo para o mesmo **Dono do Documento**

### 3.4 Regras de Versionamento
- **RN012:** Quando um documento for atualizado, a versão anterior deve ser marcada como "Inativa"
- **RN013:** Apenas uma versão de cada documento pode estar "Ativa" por vez dentro do tenant
- **RN014:** Versões inativas devem ser mantidas no sistema para auditoria

### 3.5 Regras de Segurança
- **RN015:** Usuários só podem ser autenticados em seu tenant específico
- **RN016:** Todas as operações devem validar se o usuário pertence ao mesmo tenant dos dados acessados
- **RN017:** Tentativas de acesso cross-tenant devem ser bloqueadas e registradas

## 4. Funcionalidades

### 4.1 Gestão de Tenant
- **F001:** Cadastrar, editar, consultar e gerenciar status de **Tenants**
- **F002:** Configurar parâmetros específicos por tenant
- **F003:** Monitorar uso e estatísticas por tenant

### 4.2 Gestão de Usuários
- **F004:** Cadastrar, editar, consultar e excluir **Usuários** dentro do tenant
- **F005:** Gerenciar perfis e permissões por tenant
- **F006:** Autenticação isolada por tenant

### 4.3 Gestão de Tipos
- **F007:** Cadastrar, editar, consultar e excluir **Tipos de Dono** dentro do tenant
- **F008:** Cadastrar, editar, consultar e excluir **Tipos de Documento** dentro do tenant
- **F009:** Vincular/desvincular **Tipos de Documento** a **Tipos de Dono** dentro do tenant

### 4.4 Gestão de Donos
- **F010:** Cadastrar, editar, consultar e excluir **Donos do Documento** dentro do tenant
- **F011:** Listar documentos por **Dono do Documento** dentro do tenant

### 4.5 Gestão de Documentos
- **F012:** Fazer upload de documentos dentro do tenant
- **F013:** Vincular documentos a **Donos do Documento** do mesmo tenant
- **F014:** Editar metadados dos documentos dentro do tenant
- **F015:** Excluir documentos (exclusão lógica) dentro do tenant
- **F016:** Fazer download de documentos do tenant
- **F017:** Consultar histórico de versões dentro do tenant
- **F018:** Ativar/inativar versões de documentos dentro do tenant

### 4.6 Controle de Acesso e Auditoria
- **F019:** Sistema deve ter controle de permissões por usuário e tenant
- **F020:** Registrar todas as operações para auditoria isolada por tenant
- **F021:** Relatórios de auditoria por tenant

## 5. Requisitos Técnicos

### 5.1 Isolamento de Dados
- **RT001:** Todas as consultas SQL devem incluir WHERE tenant_id = ?
- **RT002:** Criar índices compostos incluindo tenant_id para performance
- **RT003:** Implementar middleware/filtro automático para isolamento de tenant

### 5.2 Armazenamento
- **RT005:** Arquivos devem ser armazenados com estrutura de diretórios separada por tenant
- **RT006:** Sistema deve gerar chave única incluindo tenant para localização de cada arquivo
- **RT007:** Metadados dos documentos devem ser armazenados em banco de dados com isolamento por tenant
- **RT008:** Backup e restore devem ser possíveis por tenant individual

### 5.3 Auditoria
- **RT009:** Todas as entidades devem ter campos de auditoria incluindo tenant_id
- **RT010:** Todas as operações devem ser registradas em log de auditoria isolado por tenant
- **RT011:** Logs devem incluir informações de contexto do tenant

### 5.4 Identificadores e Segurança
- **RT012:** Todos os IDs únicos devem ser gerados automaticamente pelo sistema
- **RT013:** IDs devem ser imutáveis após criação
- **RT014:** Implementar validação automática de tenant em todas as operações
- **RT015:** Sessões de usuário devem incluir informação do tenant
- **RT016:** URLs/APIs devem incluir identificação do tenant quando necessário

### 5.5 Performance e Escalabilidade
- **RT017:** Sistema deve suportar múltiplos tenants sem degradação de performance
- **RT018:** Implementar cache isolado por tenant quando necessário
- **RT019:** Monitorar uso de recursos por tenant

## 6. Fluxos Principais

### 6.1 Autenticação Multi-Tenant
1. Usuário informa tenant, login e senha
2. Sistema valida credenciais e tenant
3. Sistema cria sessão incluindo informações do tenant
4. Todas as operações subsequentes são filtradas pelo tenant da sessão

### 6.2 Incluir Novo Documento
1. Usuário (autenticado no tenant) seleciona **Dono do Documento**
2. Sistema lista apenas **Donos do Documento** do tenant atual
3. Usuário seleciona **Tipo de Documento**
4. Sistema lista apenas **Tipos de Documento** do tenant atual
5. Sistema valida se o **Tipo de Documento** está vinculado ao **Tipo de Dono** dentro do tenant
6. Se **Tipo de Documento** não permite múltiplos, sistema verifica se já existe documento ativo do mesmo tipo no tenant
7. Usuário faz upload do arquivo
8. Sistema armazena arquivo em diretório específico do tenant
9. Sistema cria registro de **Documento** com tenant_id
10. Sistema registra operação na auditoria do tenant

### 6.3 Atualizar Versão de Documento
1. Usuário seleciona documento existente (apenas do seu tenant)
2. Usuário faz upload da nova versão
3. Sistema marca versão anterior como "Inativa" dentro do tenant
4. Sistema cria novo registro com status "Ativo" e tenant_id
5. Sistema registra operação na auditoria do tenant

### 6.4 Isolamento de Dados
1. Toda consulta ao banco deve incluir filtro automático por tenant_id
2. Middleware de aplicação deve validar tenant em cada requisição
3. Tentativas de acesso cross-tenant devem ser bloqueadas e auditadas
4. Cache deve ser segmentado por tenant

## 7. Considerações de Implementação

### 7.1 Estratégias de Multi-Tenancy
- **Shared Database, Shared Schema:** Todas as tabelas incluem tenant_id
- **Application Level:** Filtros automáticos na camada de aplicação

### 7.2 Configuração por Tenant
- Cada tenant pode ter configurações específicas
- Tipos de documento e dono podem variar por tenant
- Regras de negócio podem ser customizáveis por tenant

