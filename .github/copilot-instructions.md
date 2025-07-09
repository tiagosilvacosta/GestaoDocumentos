# Boas Práticas para em Projetos .NET

## Configuração Inicial

### Estrutura do Projeto

- Utilize uma estrutura de diretórios clara e organizada:

```
MeuProjeto/
├── src/
│   ├── MeuProjeto.Dominio/
│   ├── MeuProjeto.Infra/
│   ├── MeuProjeto.Api/
│   ├── MeuProjeto.TestesUnitarios/
│   └── MeuProjeto.TestesIntegrados/
├── docs/
├── .gitignore
├── README.md
└── MeuProjeto.sln
```


### Pacotes a serem usados
- Use o pacote NuGet Tsc.DddBase para facilitar a implementação de DDD e boas práticas de desenvolvimento.

### Comentários e Documentação

- Documente seu código com comentários XML para melhorar as sugestões do Copilot:

```
/// <summary>
/// Processa os dados de entrada e retorna o resultado formatado.
/// </summary>
/// <param name="input">Dados de entrada a serem processados</param>
/// <returns>Resultado formatado como string</returns>
public string ProcessarDados(string input)
{
    // Implementação
}
```

## Padrões de Codificação

- Os nomes dos métodos devem ser em PascalCase  
- Nomes de variáveis devem ser em camelCase  
- Métodos assíncronos devem ter o sufixo "Async"  
- Os nomes dos métodos e variáveis devem ser sempre em português do Brasil  
- Use os conceitos SOLID e do Clean Code para criar código de fácil manutenção  
- As classes de serviço devem ter sempre uma Interface associada  
- Usar injeção de dependências (Microsoft.Extensions.DependencyInjection)  
- Usar os padrões do Domain Driven Design  
- Os subdiretórios dos componentes devem ser nomeados de acordo com o domínio e não com as questões técnicas (Por exemplo: não devem ter diretório "serviços" ou "repositório" e sim diretórios que expliquem o domínio que estamos tratando, como por exemplo, "contrato" ou "proposta" e, dentro deles, os serviços, modelos, repositórios, interfaces, etc...)

## Boas Práticas de Código

### Tratamento de Erros

- Sempre tratar as exceções e logar as mesmas fornecendo o máximo de informações para que o erro seja identificado e resolvido  
- Sempre checar potenciais erros como, por exemplo, acesso a um índice de array inexistente.

### Testes Unitários

- Sempre crie testes unitários para testar cada uma das funções  
- Use o componente NSubstitute para fazer o mock das classes que a classe de teste precisa para ser testada

### Padrões de Projeto

- Usar o padrão de Repositório para acesso aos dados

## Segurança e Boas Práticas

### Validação de Entrada

- Validar todas as entradas de métodos para que contenham valores válidos

## Arquivos de Projeto

### .gitignore

- Todo projeto .net deve ter  um arquivo `.gitignore` adequado para projetos .NET

### README.md

- Todos projetos precisam ter README.md e ser sempre atualizado com as modificações mais recentes

## Arquitetura DDD \- Diretrizes para Desenvolvimento

### Princípios Gerais

- **Abordagem**: Domain Driven Design (DDD)  
- **Separação**: Organização por domínio de negócio, não por aspectos técnicos  
- **Dependências**: Domínios não podem depender de outros domínios ou infraestrutura

---

### 📁 Estrutura de Projetos

#### **Domínio**

```
Nome_do_dominio.Dominio/
├── ContratoDeAluguel/
│   ├── Contrato.cs (entidade)
│   ├── IRepositorioContrato.cs (interface)
│   ├── ServicoContrato.cs (serviço de domínio)
└── Inquilino/
    ├── Inquilino.cs
    └── ...
```

**Regras:**

- ✅ Organizar por área de negócio (ContratoDeAluguel, Inquilino)  
- ❌ NÃO organizar por tipo técnico (Services/, Interfaces/, Entities/)  
- ❌ NÃO referenciar outros domínios ou infraestrutura

#### **Infraestrutura** (Opcional)

```
Nome_do_dominio.Infra/
├── ContratoDeAluguel/
├── Inquilino/
└── ContratoCompraVenda/
```

**Responsabilidades:**

- Acesso a banco de dados  
- Integração com serviços externos  
- Manipulação de arquivos

#### **Apresentação do Domínio** (Opcional)

```
Nome_do_dominio.Apresentacao/
├── GeradorDePdf/
├── GeradorDeExcel/
└── GeradorDeZip/
```

---

### 🎯 Camada de Aplicação

**Projeto:** `NomeSistema.Aplicacao`

**Responsabilidades:**

- Orquestrar múltiplos domínios  
- ❌ NÃO conter lógica de negócio  
- ✅ Apenas coordenar chamadas entre domínios

**Organização:**

```c#
// Exemplo de serviço de aplicação
public class ProcessamentoContratoAppService
{
    // Coordena: Dominio.Contrato + Dominio.Financeiro + Dominio.Notificacao
}
```

---

### 🌐 Camada de Apresentação

**Tipos de Projeto:**

- `NomeSistema.Web` (MVC/Razor Pages)  
- `NomeSistema.Api` (Web API)

**Responsabilidades:**

- Receber requisições (Request)  
- Chamar camada de aplicação  
- Transformar respostas (Response)  
- ❌ NÃO conter lógica de negócio

---

### 📋 Regras de Implementação

| Cenário | Onde Implementar | Exemplo |
| :---- | :---- | :---- |
| Método usa apenas 1 classe | Na própria classe do domínio | `contrato.CalcularValorTotal()` |
| Método usa múltiplas classes do mesmo domínio | Serviço de domínio | `contratoService.ValidarContrato(contrato, inquilino)` |
| Método usa múltiplos domínios | Serviço de aplicação | `ProcessamentoContratoAppService` |

---

### 🔧 Convenções de Nomenclatura

- **Domínio:** `Contratos.Dominio`, `Financeiro.Dominio`  
- **Infraestrutura:** `Contratos.Infra`, `Financeiro.Infra`  
- **Apresentação:** `Contratos.Apresentacao`  
- **Aplicação:** `MeuSistema.Aplicacao`  
- **Web:** `MeuSistema.Web` ou `MeuSistema.Api`

---

### ✅ Checklist de Validação

Antes de criar/modificar código, verificar:

- [ ] Lógica de negócio está no domínio correto?  
- [ ] Não há dependência entre domínios?  
- [ ] Orquestração entre domínios está na aplicação?  
- [ ] Apresentação apenas transforma dados?  
- [ ] Organização é por área de negócio, não técnica?


## Projetos de API

- Todas as respostas da API precisam ser herdadas da classe *ResponseBaseModel*, para padronização dos clientes  
- Não deve haver nenhuma lógica implementada nos controllers. O controller deve usar apenas um serviço de aplicação, que fará  
- As chamadas de Api precisam ser todas autorizadas usando tokens JWT (Exceto se tivermos uma chamada para a criação de um token / login.


## Acesso à Banco de Dados:

- Usar o Entity Framework Core para acesso a banco de dados
- As classes de acesso a dados devem ser sempre implementadas dentro do diretório Infra, e não no domínio
