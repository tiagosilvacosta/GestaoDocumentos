# Sistema de Gerenciamento de Documentos Multi-Tenant

Este é um sistema completo de gerenciamento de documentos multi-tenant desenvolvido com .NET 8, seguindo os princípios de Clean Architecture e Domain-Driven Design (DDD).

## 🏗️ Arquitetura

O projeto está organizado em camadas:

- **Domain**: Entidades, Value Objects, Repositórios (interfaces), Serviços de Domínio
- **Application**: DTOs, Serviços de Aplicação, Validators, Mappings
- **Infrastructure**: Implementação de Repositórios, DbContext, Serviços de Infraestrutura
- **API**: Controllers, Middleware, Configurações

## 🛠️ Tecnologias Utilizadas

- **.NET 8**
- **Entity Framework Core 8**
- **SQL Server**
- **AutoMapper**
- **FluentValidation**
- **xUnit** (Testes)
- **FluentAssertions** (Testes)
- **Bogus** (Dados de teste)

## 📁 Estrutura do Projeto

```
src/
├── Tsc.GestaoDocumentos.Domain/          # Camada de Domínio
│   ├── Common/                           # Classes base e interfaces
│   ├── Entities/                         # Entidades do domínio
│   ├── Enums/                           # Enumerações
│   ├── Repositories/                     # Interfaces dos repositórios
│   └── Services/                        # Interfaces dos serviços de domínio
├── Tsc.GestaoDocumentos.Application/     # Camada de Aplicação
│   ├── Commands/                        # Commands CQRS
│   ├── DTOs/                           # Data Transfer Objects
│   ├── Handlers/                       # Command/Query Handlers
│   ├── Mappings/                       # AutoMapper Profiles
│   ├── Queries/                        # Queries CQRS
│   └── Validators/                     # FluentValidation
├── Tsc.GestaoDocumentos.Infrastructure/  # Camada de Infraestrutura
│   ├── Data/                           # DbContext e Configurações
│   ├── Repositories/                   # Implementações dos repositórios
│   └── Services/                       # Implementações dos serviços
└── Tsc.GestaoDocumentos.Api/            # Camada de Apresentação
    └── Controllers/                     # Controllers da API

tests/
├── Tsc.GestaoDocumentos.Domain.Tests/    # Testes de Domínio
├── Tsc.GestaoDocumentos.Application.Tests/ # Testes de Aplicação
└── Tsc.GestaoDocumentos.Infrastructure.Tests/ # Testes de Infraestrutura
```

## 🗂️ Entidades Principais

### Multi-Tenancy
- **Tenant**: Organização/cliente com isolamento completo de dados
- **Usuario**: Usuários vinculados a um tenant específico

### Gerenciamento de Documentos
- **TipoDono**: Categorias de proprietários (Pessoa Física, Jurídica, etc.)
- **TipoDocumento**: Tipos de documentos (RG, CPF, Nota Fiscal, etc.)
- **DonoDocumento**: Container que agrupa documentos de um proprietário
- **Documento**: Arquivo com metadados e versionamento
- **LogAuditoria**: Registro de todas as operações

## 🔐 Características de Segurança

### Multi-Tenancy
- Isolamento completo de dados por tenant
- Filtros automáticos nas consultas
- Validação de tenant em todas as operações
- Estrutura de armazenamento separada por tenant

### Auditoria
- Log completo de todas as operações
- Rastreamento de usuário, IP e User Agent
- Versionamento de documentos
- Histórico de alterações

### Criptografia
- Hash seguro de senhas com PBKDF2
- Salt único para cada senha
- Verificação de senha com tempo constante

## 🚀 Como Executar

### Pré-requisitos
- .NET 8 SDK
- SQL Server (LocalDB ou instância completa)
- Visual Studio 2022 ou VS Code

### Configuração do Banco de Dados

1. Ajuste a connection string no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GestaoDocumentosDB;Trusted_Connection=true;MultipleActiveResultSets=true"
  }
}
```

2. Execute as migrations:
```bash
cd src/Tsc.GestaoDocumentos.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../Tsc.GestaoDocumentos.Api
dotnet ef database update --startup-project ../Tsc.GestaoDocumentos.Api
```

### Executando a Aplicação

```bash
cd src/Tsc.GestaoDocumentos.Api
dotnet run
```

A API estará disponível em `https://localhost:7000` e `http://localhost:5000`.

## 🧪 Executando os Testes

```bash
# Todos os testes
dotnet test

# Testes específicos
dotnet test tests/Tsc.GestaoDocumentos.Domain.Tests/
dotnet test tests/Tsc.GestaoDocumentos.Application.Tests/
dotnet test tests/Tsc.GestaoDocumentos.Infrastructure.Tests/
```

## 📚 Documentação da API

Após executar a aplicação, acesse:
- Swagger UI: `https://localhost:7000/swagger`

## 🔧 Configurações

### Armazenamento de Arquivos
```json
{
  "ArmazenamentoArquivos": {
    "CaminhoBase": "C:\\temp\\gestao-documentos\\uploads"
  }
}
```

### Logging
O sistema utiliza Serilog para logging estruturado. Logs são salvos em arquivos e console.

## 📋 Regras de Negócio Implementadas

- **RN001-RN004**: Isolamento multi-tenant
- **RN005-RN008**: Regras de vinculação entre entidades
- **RN009-RN011**: Validações de tipos e múltiplos documentos
- **RN012-RN014**: Versionamento de documentos
- **RN015-RN017**: Segurança e validação de tenant

## 🤝 Contribuição

1. Fork o projeto
2. Crie uma branch para sua feature (`git checkout -b feature/AmazingFeature`)
3. Commit suas mudanças (`git commit -m 'Add some AmazingFeature'`)
4. Push para a branch (`git push origin feature/AmazingFeature`)
5. Abra um Pull Request

## 📄 Licença

Este projeto está sob a licença MIT. Veja o arquivo `LICENSE` para mais detalhes.
